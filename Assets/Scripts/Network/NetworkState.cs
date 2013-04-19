using UnityEngine;
using System.Collections;

public class NetworkState {
	
	public float timestamp; // The Time this state occured on the network
	public Vector3 pos; // Position of the attached object at that time
	public Quaternion rot; // Rotation at that time
	
	public NetworkState() {
		timestamp = 0.0f;
		pos = Vector3.zero;
		rot = Quaternion.identity;
	}
	
	public NetworkState( float time, Vector3 position, Quaternion rotation) {
		timestamp = time;
		pos = position;
		rot = rotation;
	}
}
