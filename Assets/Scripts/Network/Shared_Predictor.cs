using UnityEngine;
using System.Collections;

public class Shared_Predictor : MonoBehaviour {
	
	public Transform observedTranform;
	public Client_PlayerManager receiver; // Guy who is receiving data
	public float pingMargin = 0.1f; // ping top-margin
	
	private float clientPing;
	private NetworkState[] serverStateBuffer = new NetworkState[20];
	
	public void OnSerializeNetworkView( BitStream stream, NetworkMessageInfo info) {
		Vector3 pos = observedTranform.position;
		Quaternion rot = observedTranform.rotation;
		
		if(stream.isWriting) {
			// Debug.Log("Server is writing!");
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		} else {
			// This code takes care of the local client!
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			receiver.serverPos = pos;
			receiver.serverRot = rot;
			// Smoothly correct clients position.
			// receiver.lerpToTarget(); DONT NEED THIS ANY LONGER SINCE WE USE ONLY INTERPOLATION FOR ALL PLAYER
			
			// Take care of data for interpolation remote objects movements
			// Shift up the buffer
			for( int i = serverStateBuffer.Length - 1; i >= 1; i--) {
				serverStateBuffer[i] = serverStateBuffer[i-1];	
			}
			// Override the first element with the latest serer info
			serverStateBuffer[0] = new NetworkState((float) info.timestamp, pos, rot);
		}
	}
	
	public void Update() {
		// Do the same interpolation for all player
		// There is no prediction
		if (/*(Network.player == receiver.GetPlayer()) ||*/ Network.isServer) {
			return; // This is only for remote peers, get off	
		}
		// Client side has !!only the server connected!!
		clientPing = (Network.GetAveragePing(Network.connections[0]) / 100) + pingMargin;
		float interpolationTime = (float) Network.time - clientPing;
		// Ensure the buffer has at least one elemnt:
		if (serverStateBuffer[0] == null) {
			serverStateBuffer[0] = new NetworkState(0,
										transform.position,
										transform.rotation);
		}
		
		// Debug.Log("Ping: " + clientPing);
		// Try interpolation if possible.
		// If the latest serverStateBuffer timestamp is smaller than the latency
		// we're not slow enough to really lag out and just extrapolate.
		if (serverStateBuffer[0].timestamp > interpolationTime) {
			for ( int i=0; i < serverStateBuffer.Length; i++) {
				if (serverStateBuffer[i] == null) {
					continue;	
				}
				// Find the state which mathces the interp. time of use last state
				if (serverStateBuffer[i].timestamp <= interpolationTime ||
					i == serverStateBuffer.Length - 1) {
					
					// The state one frame newer that the best playback state
					NetworkState bestTarget = serverStateBuffer[Mathf.Max(i-1,0)];
					// The best playback state (closest current network time)
					NetworkState bestStart = serverStateBuffer[i];
					
					float timediff = bestTarget.timestamp - bestStart.timestamp;
					float lerpTime = 0.0f;
					// Increase the interpolation amount by growing ping.
					// Reverse that for more smooth but less accurate positioning.
					if (timediff > 0.0001) {
						lerpTime = ((interpolationTime - bestStart.timestamp) / timediff);	
					}
					// Debug.Log("Interpol, LerpTime " + lerpTime + " ping " + clientPing + " timediff " + timediff);
					
					
					transform.position = Vector3.Lerp(bestStart.pos, bestTarget.pos,lerpTime);
					transform.rotation = Quaternion.Slerp(bestStart.rot, bestTarget.rot, lerpTime);
					// Okay found our way though to lerp the position, lets retun here
					return;
				}
			}
		} else { // So it appears there is no lag thourgh latency.
			float lerpTime = 0.2f;
			NetworkState latest = serverStateBuffer[0];
			transform.position = Vector3.Lerp(transform.position, latest.pos, lerpTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, latest.rot, lerpTime);
		
			// Debug.Log("Difference: " + (transform.position - serverStateBuffer[0].pos).magnitude);
		}
	}
}
