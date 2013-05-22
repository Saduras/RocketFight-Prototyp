using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;

public class SFS2X_Connect : MonoBehaviour {
	
	public string serverIP = "127.0.0.1";
	public int serverPort = 9933;
	
	private SmartFox sfs;

	// Use this for initialization
	void Start () {
		sfs = new SmartFox();
		sfs.ThreadSafeMode = true; // hold events until we ask for them
		sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		
		sfs.Connect(serverIP,serverPort);
	}
	
	// Update is called once per frame
	void Update () {
		sfs.ProcessEvents();
	}
	
	void OnConnection(BaseEvent e) {
		if ((bool) e.Params["success"]) {
			Debug.Log("Successfully connected!");	
		} else {
			Debug.Log("Connection failed!");	
		}
	}
	
	void OnApplicationQuit() {
		if (sfs.IsConnected) {
			sfs.Disconnect();	
		}
	}
}
