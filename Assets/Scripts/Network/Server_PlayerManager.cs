using UnityEngine;
using System.Collections;

public class Server_PlayerManager : MonoBehaviour {

	public float speed = 10;
	
	private Vector3 movement = Vector3.zero;
	private Quaternion rotation = Quaternion.identity;
	
	// Use this for initialization
	public void Start() {
	}
	
	// Update is called once per frame
	public void Update() {
		// Check if we are on the server-side
		// otherwise get lost!
		if (Network.isClient)
			return;
		
		this.transform.Translate(movement * speed * Time.deltaTime, Space.World);
		this.transform.rotation = rotation;
	}
	
	[RPC]
	public void UpdateMovement(Vector3 direction) {
		movement = new Vector3(direction.x, 0, direction.y);
	}
	
	[RPC]
	public void UpdateRotation(Vector3 viewDirection) {
		// calculate the angle between viewDirection and the screen-y-axis
		float angle = Vector2.Angle(Vector2.up,(Vector2)viewDirection);
		
		// since Vector2.Angle always gives the smalles angle, we need to
		// invert the angle for viewDirection on the left half circle
		if(viewDirection.x < 0)
			angle = 360.0f - angle;
		
		rotation = Quaternion.Euler(new Vector3(0,angle,0));
	}
	
}
