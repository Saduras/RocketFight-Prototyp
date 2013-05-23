using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;

public class SFS2X_Connect : MonoBehaviour {
	
	public string configFile = "Scripts/Network/sfs-config.xml";
	public bool useConfigFile = false;
	public string serverIP = "127.0.0.1";
	public int serverPort = 9933;
	public string zoneName = "BasisExamples";
	public string userName = "";
	public string roomName = "The Lobby"; 
	
	private SmartFox sfs;

	// Use this for initialization
	void Start () {
		sfs = new SmartFox();
		sfs.ThreadSafeMode = true; // hold events until we ask for them
		sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		sfs.AddEventListener(SFSEvent.CONFIG_LOAD_SUCCESS, OnConfigLoad); 
        sfs.AddEventListener(SFSEvent.CONFIG_LOAD_FAILURE, OnConfigFail);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom); 
        sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnJoinRoomError); 
		
		if( useConfigFile ) {
			sfs.LoadConfig( Application.dataPath + "/" + configFile );	
		} else {
			sfs.Connect(serverIP,serverPort);
		}
	}
	
	void OnConfigLoad(BaseEvent e) {
		Debug.Log("Config loaded successfully");
		serverIP = sfs.Config.Host;
		serverPort = sfs.Config.Port;
		zoneName = sfs.Config.Zone;
		
		sfs.Connect(serverIP,serverPort);
	}
	
	void OnConfigFail(BaseEvent e) { 
        Debug.Log("Failed to load Config File"); 
    } 
	
	void OnLogin(BaseEvent e) {
		Debug.Log("Logged in: " + e.Params["user"]);
		sfs.Send(new JoinRoomRequest(roomName)); 
	}
	
	void OnJoinRoom(BaseEvent e) 
    { 
        Debug.Log("Joined Room: " + e.Params["room"]); 
    } 
     
    void OnJoinRoomError(BaseEvent e) 
    { 
        Debug.Log("JoinRoom Error (" + e.Params["errorCode"] + "): " + e.Params["errorMessage"]); 
    } 
	
	void OnLoginError(BaseEvent e) {
		Debug.Log("Login Error (" + e.Params["errorCode"] + ") : " + e.Params["errorMessage"]);	
	}
	
	void OnConnection(BaseEvent e) {
		if ((bool) e.Params["success"]) {
			Debug.Log("Successfully connected!");	
			sfs.Send(new LoginRequest(userName, "", zoneName)); 
		} else {
			Debug.Log("Connection failed!");	
		}
	}
	
	// Update is called once per frame
	void Update () {
		sfs.ProcessEvents();
	}
	
	void OnApplicationQuit() {
		if (sfs.IsConnected) {
			sfs.Disconnect();	
		}
	}
}
