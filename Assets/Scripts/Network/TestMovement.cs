using UnityEngine;
using System.Collections;

public class TestMovement : MonoBehaviour {
	
	public Vector3 pos1 = new Vector3(-1,0,1);
	public Vector3 pos2 = new Vector3(1,0,1);
	public float speed = 1.0f;
	
	private float t = 0.0f;
	private bool increasing = true;

	// Use this for initialization
	void Start () {
		this.transform.position = pos1;
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer) {
			if(increasing) {
				t += speed * Time.deltaTime;
				if(t >= 1)
					increasing = false;
			}
			
			if(!increasing) {
				t -= speed * Time.deltaTime;
				if(t <= 0)
					increasing = true;	
			}
			
			this.transform.position = (1-t) * pos1 + t *  pos2;
		}
	}
}
