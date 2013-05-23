using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public float positionErrorThreshold = 0.2f;
	// Real position of character on server.
	public Vector3 serverPos;
	// Real view direction of the character on server. 
	public Quaternion serverRot;
	
	public Texture2D cursorTex;
	public int cursorSizeX = 32;  // set to width of your cursor texture
	public int cursorSizeY= 32;  // set to height of your cursor texture
	
	// The client who controls this character
	private PhotonPlayer controllingPlayer;
	
	// Previous movement and view direction used to
	// prevent script from sending data if there is no change.
	private Vector3 lastMoveDirection;
	private Vector3 lastViewDirection;
	
	// This is the movement speed of the character used for prediction.
	// Get the value from the server script.
	private float speed = 5;
	
	/**
	 * This is called at the very beginning.
	 * Immediately disable this if you are a client. Will be
	 * enabled once the controlling client was set.
	 */
	public void Awake() {
		//speed = this.gameObject.GetComponent<Server_PlayerManager>().speed;
		Screen.showCursor = false;
	}
	
	/**
	 * Is called once per frame. For Client only!
	 * Get user input and if there is any change to previous input 
	 * sent it to server and make own prediction.
	 */
	public void Update () {
		
		// Check for input updates
		if( PhotonNetwork.player == controllingPlayer) {
			// Get movement input.
			Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
			Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y);
			//Debug.Log(this.transform.position + " + " + movement);
			// movement.Normalize();
			//this.transform.position += movement * speed * Time.deltaTime;
			this.transform.Translate(movement * speed * Time.deltaTime, Space.World);
			Debug.Log(this.transform.position);
			
			// Get rotation input.
			Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] rayHits = Physics.RaycastAll(cursorRay);
			Vector3 hitPoint = Vector3.zero;
			foreach(RaycastHit hit in rayHits) {
				if (hit.collider.CompareTag("ground")) {
					hitPoint = hit.point;
				}
			}
			Vector3 viewDirection = hitPoint - this.transform.position;
			viewDirection.y = 0;
			viewDirection.Normalize();
			this.transform.LookAt(this.transform.position + viewDirection);

			
			// Get fire input.
			// if (Input.GetButtonDown("Fire"))
			//	networkView.RPC("ShootMissile",RPCMode.Server, true);
			// if (Input.GetButtonUp("Fire"))
			//	networkView.RPC("ShootMissile",RPCMode.Server, false);
		}
	}
	
	/**
	 * Replace mouse coursor with custom texture
	 */
	public void OnGUI(){
		GUI.DrawTexture( new Rect(Input.mousePosition.x-cursorSizeX/2, 
				(Screen.height-Input.mousePosition.y)-cursorSizeY/2, 
				cursorSizeX, 
				cursorSizeY),cursorTex);
	}
	
	/**
	 * Move smoothly to server state if the distance is to big.
	 * Use linear transformation for smooth movement.
	 */
	public void lerpToTarget() {
		float distance = Vector3.Distance(transform.position, serverPos);
		// Debug.Log("Lerp by " + distance);
		
		// Only correct if the error margin (the distance) is to extreme.
		if (distance >+ positionErrorThreshold) {
			float lerp = ((1 / distance) * speed) /100;
			// Debug.Log("Lerp time: " + lerp);
			transform.position = Vector3.Lerp(transform.position, serverPos, lerp);
			transform.rotation = Quaternion.Slerp(transform.rotation, serverRot, lerp);
		}
	}
	
	/**
	 * Called by server via RPC. Define the client who will control this character
	 * and re-enable this script for the client.
	 */
	[RPC]
	public void SetPlayer(PhotonPlayer player) {
		Debug.Log("Setting the controlling player");
		controllingPlayer = player;
		//if(player == Network.player) {
			// We are now sure that WE are the player in control
			// time to enabgle the controls again
		//	enabled = true;	
			//GameObject.Find("NetworkManager").GetComponent<Client_NetworkManager>().localPlayer = this.gameObject;
		//}
	}
	
	/**
	 * Allows the sever to ask who controls this character.
	 */
	[RPC]
	public PhotonPlayer GetPlayer() {
		return controllingPlayer;	
	}
	
	[RPC]
	public void SetColor(Vector3 colorVec) {
		Color col = new Color(colorVec.x, colorVec.y, colorVec.z, 1);
		this.renderer.material.SetColor("_Color",col);
	}
}
