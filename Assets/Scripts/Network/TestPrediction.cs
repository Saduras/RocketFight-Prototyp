using UnityEngine;
using System.Collections;

public class TestPrediction : MonoBehaviour {
	
	private NetworkState[] serverStateBuffer = new NetworkState[20];
	
	public void OnSerializeNetworkView( BitStream stream, NetworkMessageInfo info) {
		Vector3 pos = this.transform.position;
		Quaternion rot = Quaternion.identity;
		
		if(stream.isWriting) {
			// Debug.Log("Server is writing!");
			stream.Serialize(ref pos);
		} else {
			// This code takes care of the local client!
			stream.Serialize(ref pos);
			
			// Take care of data for interpolation remote objects movements
			// Shift up the buffer
			for( int i = serverStateBuffer.Length - 1; i >= 1; i--) {
				serverStateBuffer[i] = serverStateBuffer[i-1];	
			}
			// Override the first element with the latest serer info
			serverStateBuffer[0] = new NetworkState((float) info.timestamp, pos, rot);
		}	
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*Debug.Log("======================");
		for(int i=0; i<serverStateBuffer.Length; i++) {
			Debug.Log("[" + i + "] " + serverStateBuffer[i].pos);
		}
		Debug.Log("======================");*/
		if(Network.isClient) {
			NetworkState latest = serverStateBuffer[0];
			transform.position = Vector3.Lerp(transform.position, latest.pos, 0.2f);
			// Use Ping for Lerp time?
			// Debug.Log("Ping: " + Network.GetLastPing(Network.connections[0]));
		}
	}
}
