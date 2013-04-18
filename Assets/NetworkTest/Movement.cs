using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isClient) {
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			
			Vector3 v = new Vector3(x,0,y);
			
			
			// this.transform.Translate( new Vector3(1,-1,0) * Time.deltaTime);
			this.networkView.RPC("Move",RPCMode.Others, v * Time.deltaTime);
			//this.transform.Translate(v * Time.deltaTime);
		}
	}
	
	// javascript: @RPC
	[RPC] // c#
	void Move(Vector3 movement) {
		this.transform.Translate(movement);	
	}
}
