using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
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
	
	// dem player gehört das Objekt
	void OnConnectedToServer() {
	//	this.CreatePlayer();
	}
	
	// dem server gehört das Objekt
	void OnPlayerConnected() {
		this.CreatePlayer();
	}
	
	void OnServerInitialized(){
		this.CreatePlayer();	
	}
	
	void CreatePlayer() {
		Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
	}
}
