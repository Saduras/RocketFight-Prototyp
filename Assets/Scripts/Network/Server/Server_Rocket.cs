using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Server_Rocket : MonoBehaviour {
	
	public float speed = 10.0f;
	public float lifeTime = 3.0f;
	public float explosionStrengh = 3.0f;
	public float explosionRadius = 3.0f;
	public float explosionUpwardsMod = 0.0f;
	
	private float timer = 0.0f;

	// Use this for initialization
	public void Start () {
		// This script is for server only! Get out if you are not!
		if (Network.isClient) {
			enabled = false;	
		}
			
			
		timer = Time.time;
		this.rigidbody.velocity = this.transform.TransformDirection( new Vector3(0,0,speed) );
	}
	
	// Update is called once per frame
	public void Update () {
		if(Network.isServer && Time.time > timer + lifeTime) {
			Network.Destroy(this.gameObject);
			Network.RemoveRPCs(this.networkView.viewID);
		}
	}
	
	public void OnCollisionEnter(Collision collision) {
		Debug.Log("Boom!");
		if(Network.isClient) {
			GameObject playerObj = GameObject.Find("NetworkManager").GetComponent<Client_NetworkManager>().localPlayer;
			Vector3 force = playerObj.transform.position - this.transform.position;
			float distance = force.magnitude;
			force.Normalize();
			float scaleFactor = explosionStrengh / distance;
			force.Scale(new Vector3(scaleFactor,scaleFactor,scaleFactor) );
				
			playerObj.SendMessage("SetExplosionForce",force);
			//playerObj.gameObject.GetComponent<Shared_Predictor>().SendMessage("SetExplosionForce",force);
		}
		
		
		if(Network.isServer) {
			
			List<Client_PlayerManager> playerList = GameObject.Find("NetworkManager").GetComponent<Server_NetworkManager>().GetPlayerList();
			foreach( Client_PlayerManager player in playerList ) {
				Debug.Log(player.gameObject.networkView.viewID);
				Vector3 force = player.transform.position - this.transform.position;
				float distance = force.magnitude;
				force.Normalize();
				float scaleFactor = explosionStrengh / distance;
				force.Scale(new Vector3(scaleFactor,scaleFactor,scaleFactor) );
				
				player.SendMessage("SetExplosionForce",force);
				//player.gameObject.GetComponent<Shared_Predictor>().SendMessage("SetExplosionForce",force);
				/*player.gameObject.rigidbody.AddExplosionForce( explosionForce,
															this.transform.position,
															explosionRadius,
															explosionUpwardsMod,
															ForceMode.Impulse);*/
			}
			Network.Destroy(this.gameObject);
			Network.RemoveRPCs(this.networkView.viewID);
		}
	}
}
