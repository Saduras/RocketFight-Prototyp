using UnityEngine;
using System.Collections;

public class Server_PlayerPhysik : MonoBehaviour {
	
	// Stuff for the expolsion physics
	private Vector3 explosionForce = Vector3.zero;
	private float explosionEpsilon =  0.02f;
	public static float explosionFadeTime = 1f; // Time it takes to reduce explosionForce to zero
	private Vector3 explosionFade = Vector3.zero;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.ApplyForce();
	}
	
		
	private void ApplyForce() {
		if(explosionForce.magnitude > explosionEpsilon) {
			Debug.Log(explosionForce);
			// Apply explosion force
			this.transform.Translate(explosionForce * Time.deltaTime,Space.World);
			// Weaken explosion for the next update
			Vector3 forceDelta = explosionFade * Time.deltaTime;
			if(explosionForce.magnitude > forceDelta.magnitude) {
				explosionForce = explosionForce - forceDelta;
			} else {
				explosionForce = Vector3.zero;	
			}
		}
	}
	
	public void SetExplosionForce(Vector3 force) {
		this.explosionForce = force + explosionForce;
		this.explosionFade = explosionForce.normalized * 1/explosionFadeTime;
	}
}
