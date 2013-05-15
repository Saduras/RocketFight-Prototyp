using UnityEngine;
using System.Collections;

public class Client_NetworkManager : MonoBehaviour {
	
	public GameObject localPlayer;

	public void OnConnectedToServer() {
		Debug.Log("Disabling message queue!");
		Network.isMessageQueueRunning = false;
		Application.LoadLevel(Server_NetworkManager.levelName);
		
		
	}

	public void OnLevelWasLoaded(int level) {
		// if (level != 0 && Network.isClient) {
		if (Network.isClient) {
			Network.isMessageQueueRunning = true;
			Debug.Log("Level was loaded, requesting spawn.");
			Debug.Log("Re-enabling message queue!");
			// Request a player instance from the server
			this.networkView.RPC("requestSpawn", RPCMode.Server, Network.player);
			this.gameObject.GetComponent<GUI_Network>().enabled = false;
		}
	}
}
