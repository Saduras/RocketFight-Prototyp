using UnityEngine;
using System.Collections;

public class GUI_Network : MonoBehaviour {
	
	public string serverIP = "192.168.178.45";
	public int serverPort = 25001;
	public bool run = true;
	
	void OnGUI(){
		if ( run ) {
			GUILayout.BeginArea(new Rect(Screen.width * 0.7f, 
										Screen.height * 0.1f, 
										Screen.width * 0.2f, 
										Screen.height * 0.2f));
			
			if( GUILayout.Button("Start server") ){
				Network.incomingPassword = "myPassword";
				// param: connection number, port, useNat
				Network.InitializeServer(4, serverPort, false);
			}
			if( GUILayout.Button("Connect to server") ){
				// param: ip-address, port, password
				Network.Connect(serverIP,serverPort,"myPassword");
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
}
