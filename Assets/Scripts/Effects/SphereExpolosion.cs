using UnityEngine;
using System.Collections;

public class SphereExpolosion : MonoBehaviour {

	private bool run = true;
	private Vector3 startScale;
	private float startTime = 0.0f;
	public float time = 2.0f;
	public float size = 5.0f;
	
	
	// Use this for initialization
	void Start () {
		startScale = this.transform.localScale;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if ( run  && (time + startTime >= Time.time) ) {
			this.transform.localScale += size * startScale * Time.deltaTime / time;
		} else {
			if( Network.isServer) Network.Destroy( this.gameObject );	
		}
	}
}
