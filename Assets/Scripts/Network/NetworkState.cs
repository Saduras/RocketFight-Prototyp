using UnityEngine;
using System.Collections;

/**
 * This class is just a datatype to store any network state 
 * of an gameobject's transform component.
 */
public class NetworkState {
	
	public float timestamp; // The Time this state occured on the network
	public Vector3 pos; // Position of the attached object at that time
	public Quaternion rot; // Rotation at that time
	
	/**
	 * Default constructor. Initialize all three member with the neutral element
	 * (i.e. zero or the identity).
	 */
	public NetworkState() {
		timestamp = 0.0f;
		pos = Vector3.zero;
		rot = Quaternion.identity;
	}
	
	/**
	 * The constructor you will usually use. Set all three members via parameters.
	 */
	public NetworkState( float time, Vector3 position, Quaternion rotation) {
		timestamp = time;
		pos = position;
		rot = rotation;
	}
}
