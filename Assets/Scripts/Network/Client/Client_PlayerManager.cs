using UnityEngine;
using System.Collections;

/**
 * This class is the client side of the player manager. 
 * Send player input to server if you are the client assigned to this player object.
 * But first the server muss assigne a client to this object so it know who will control this.
 */
public class Client_PlayerManager : MonoBehaviour {
	
	public float positionErrorThreshold = 0.2f;
	// Real position of character on server.
	public Vector3 serverPos;
	// Real view direction of the character on server. 
	public Quaternion serverRot;
	
	// The client who controls this character
	private NetworkPlayer controllingPlayer;
	
	// Previous movement and view direction used to
	// prevent script from sending data if there is no change.
	private Vector3 lastMoveDirection;
	private Vector3 lastViewDirection;
	
	// This is the movement speed of the character used for prediction.
	// Get the value from the server script.
	private float speed;
	
	/**
	 * This is called at the very beginning.
	 * Immediately disable this if you are a client. Will be
	 * enabled once the controlling client was set.
	 */
	public void Awake() {
		// Disable by default for now
		// Will be enabled when controllerPlayer was set.
		if (Network.isClient) {
			enabled = false;
			speed = this.gameObject.GetComponent<Server_PlayerManager>().speed;
		}
	}
	
	/**
	 * Is called once per frame. For Client only!
	 * Get user input and if there is any change to previous input 
	 * sent it to server and make own prediction.
	 */
	public void Update () {
		// This is a client script!
		// Get lost if you are not a client!
		if (Network.isServer)
			return;
		
		// Check for input updates
		if((controllingPlayer!=null) && (Network.player == controllingPlayer)) {
			// Get movement input.
			Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
			if(moveDirection !=  lastMoveDirection) {
				networkView.RPC("UpdateMovement",RPCMode.Server,moveDirection);
				lastMoveDirection = moveDirection;
				
				Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);
				this.transform.Translate(movement * speed * Time.deltaTime, Space.World);
			}
			
			// Get rotation input.
			Vector3 mousePos = Input.mousePosition;
			Vector3 charPos = Camera.mainCamera.WorldToScreenPoint(this.transform.position);
			Vector3 viewDirection = mousePos - charPos;
			if(viewDirection != lastViewDirection) {
				networkView.RPC("UpdateRotation",RPCMode.Server,viewDirection);
				lastViewDirection = viewDirection;
				
				// calculate the angle between viewDirection and the screen-y-axis.
				float angle = Vector2.Angle(Vector2.up,(Vector2)viewDirection);
				
				// Since Vector2.Angle always gives the smalles angle, we need to
				// invert the angle for viewDirection on the left half circle.
				if(viewDirection.x < 0)
					angle = 360.0f - angle;
				
				Quaternion rotation = Quaternion.Euler(new Vector3(0,angle,0));
				this.transform.rotation = rotation;
			}
			
			// Get fire input.
			if (Input.GetButtonDown("Fire"))
				networkView.RPC("ShootMissile",RPCMode.Server, true);
			if (Input.GetButtonUp("Fire"))
				networkView.RPC("ShootMissile",RPCMode.Server, false);
		}
	}
	
	/**
	 * Move smoothly to server state if the distance is to big.
	 * Use linear transformation for smooth movement.
	 */
	public void lerpToTarget() {
		float distance = Vector3.Distance(transform.position, serverPos);
		Debug.Log("Lerp by " + distance);
		
		// Only correct if the error margin (the distance) is to extreme.
		if (distance >+ positionErrorThreshold) {
			float lerp = ((1 / distance) * (speed + this.rigidbody.velocity.magnitude )) /100;
			Debug.Log("Lerp time: " + lerp);
			transform.position = Vector3.Lerp(transform.position, serverPos, lerp);
			transform.rotation = Quaternion.Slerp(transform.rotation, serverRot, lerp);
		}
	}
	
	/**
	 * Called by server via RPC. Define the client who will control this character
	 * and re-enable this script for the client.
	 */
	[RPC]
	public void SetPlayer(NetworkPlayer player) {
		Debug.Log("Setting the controlling player");
		controllingPlayer = player;
		if(player == Network.player) {
			// We are now sure that WE are the player in control
			// time to enabgle the controls again
			enabled = true;	
		}
	}
	
	/**
	 * Allows the sever to ask who controls this character.
	 */
	[RPC]
	public NetworkPlayer GetPlayer() {
		return controllingPlayer;	
	}
}
