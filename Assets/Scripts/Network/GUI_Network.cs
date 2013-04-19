using UnityEngine;
using System.Collections;

public class GUI_Network : MonoBehaviour {

	void OnGUI(){
		GUILayout.BeginArea(new Rect(Screen.width * 0.7f, 
									Screen.height * 0.1f, 
									Screen.width * 0.2f, 
									Screen.height * 0.2f));
		
		if( GUILayout.Button("Start server") ){
			Network.incomingPassword = "myPassword";
			// param: connection number, port, useNat
			Network.InitializeServer(4, 25001, false);
		}
		if( GUILayout.Button("Connect to server") ){
			// param: ip-address, port, password
			Network.Connect("127.0.0.1",25001,"myPassword");
		}
		
		switch(Network.peerType) {
			case NetworkPeerType.Client: 
				GUILayout.TextArea("Client connected");
				break;
			case NetworkPeerType.Connecting: 
				GUILayout.TextArea("Connecting...");
				break;
			case NetworkPeerType.Disconnected: 
				GUILayout.TextArea("Disconnected");
				break;
			case NetworkPeerType.Server:
				GUILayout.TextArea("Server initialized");
				break;
			default:
				break;
		}
		
		GUILayout.EndArea();
	}
}
