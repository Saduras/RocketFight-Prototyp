using UnityEngine;
using System.Collections;

public class Server_PlayerManager : MonoBehaviour {

	public float speed = 10.0f;
	public float rocketLauncherCD = 1.0f;
	public GameObject rocketPrefab;
	
	public bool controlable = true;
	private Vector3 movement = Vector3.zero;
	private Quaternion rotation = Quaternion.identity;
	private bool shoot = false;
	private float lastShot = 0.0f;
	private GameObject spawnPoint;
	
	// Update is called once per frame
	public void Update() {
		// Check if we are on the server-side
		// otherwise get lost!
		if (Network.isClient)
			return;
		
		// Apply player movement (was set via RPC)
		if(controlable) {
			//Vector3 old = this.transform.position;
			this.transform.Translate(movement * speed * Time.deltaTime, Space.World);
			this.transform.rotation = rotation;
			//Vector3 newPos = this.transform.position;
			//Debug.Log("Movedistance: " + (old - newPos).magnitude + " Movement: " + movement);
		}
		
		// Shoot!
		if (shoot && (Time.time > lastShot + rocketLauncherCD) ) {
			lastShot = Time.time;
			Transform barrel = this.transform.Find("Barrel");
			Vector3 startPos = barrel.position;
			Quaternion startRot = this.transform.rotation;
			GameObject handle = (GameObject) Network.Instantiate(rocketPrefab,startPos,startRot,(int)NetworkGroup.SERVER);
			Color diffuse = this.renderer.material.color;
			handle.renderer.material.SetColor("_Color",diffuse);
			Vector3 colorVec = new Vector3(diffuse.r,diffuse.g,diffuse.b);
			handle.networkView.RPC("SetColor",RPCMode.AllBuffered,colorVec);
		}
	}
	
	public void SetSpawnPoint(GameObject spawnPt) {
		this.spawnPoint = spawnPt;
		Material diffuse = spawnPoint.renderer.material;
		this.gameObject.renderer.material = diffuse;
		Vector3 colorVec = new Vector3(diffuse.color.r,diffuse.color.g,diffuse.color.b);
		this.networkView.RPC("SetColor", RPCMode.AllBuffered, colorVec);
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
	
	[RPC]
	public void ShootMissile(bool fire) {
		shoot = fire;
	}
	
	public void OnDeath() {
		Debug.Log("Dead");
		this.transform.position = spawnPoint.transform.position;
	}
	
}
