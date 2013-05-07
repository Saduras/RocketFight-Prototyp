using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Server_NetworkManager : MonoBehaviour {
	
	public GameObject player;
	public static string levelName = "NetworkPrototyp";
	
	public GameObject test;
	
	private List<Client_PlayerManager> playerTracker = new List<Client_PlayerManager>();
	private List<NetworkPlayer>  scheduledSpawns = new List<NetworkPlayer> ();
	
	private bool processSpawnRequests = true;
	
	public void OnServerInitialized() {
		if(test != null) {
			Network.Instantiate(test,Vector3.zero,Quaternion.identity,(int)NetworkGroup.SERVER);
		}
	}
	
	public void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Spawning prefab for new client");
		scheduledSpawns.Add(player);
		processSpawnRequests = true;
	}

	public void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player " + player.guid + " disconnected.");
		Client_PlayerManager found = null;
		foreach( Client_PlayerManager man in playerTracker) {
			if( man.GetPlayer() == player) {
				Network.RemoveRPCs(man.gameObject.networkView.viewID);
				Network.Destroy(man.gameObject);
				found = man;
			}
		}
		if (found) {
			playerTracker.Remove(found);
		}
	}

	[RPC]
	public void requestSpawn(NetworkPlayer requester) {
		if(Network.isClient) {
			Debug.Log("Client tried to spawn itself! Revice logic!");
			return; // Get lost! This is server buisness
		}
		if(!processSpawnRequests) {
			return; // Spawn is disabled. Therefor do nothing.
		}

		foreach(NetworkPlayer spawn in scheduledSpawns) {
			Debug.Log("Checking player " + spawn.guid);
			if(spawn == requester) {
				// int num = int.Parse(spawn + "");
				GameObject handle = (GameObject) Network.Instantiate(player, 
										transform.position, 
										Quaternion.identity, 
										(int)NetworkGroup.PLAYER);
				Client_PlayerManager sc = handle.GetComponent<Client_PlayerManager>();
				if(!sc)
					Debug.Log("The prefab has no Client_PlayerManager attached!");

				playerTracker.Add(sc);
				// Get the network view of the player and add its owner
				NetworkView netView = handle.GetComponent<NetworkView>();
				netView.RPC("SetPlayer", RPCMode.AllBuffered, spawn);
			}
		}
		scheduledSpawns.Remove(requester);
		if (scheduledSpawns.Count == 0) {
			Debug.Log("Spawns is empty! Stopping spawn request processing.");
			processSpawnRequests = false;
		}  
	}
	
	[RPC]
	public List<Client_PlayerManager> GetPlayerList() {
		return playerTracker;	
	}
	
	public void OnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width * 0.2f, 
									Screen.height * 0.1f, 
									Screen.width * 0.2f, 
									Screen.height * 0.2f));
		
		foreach( Client_PlayerManager player in playerTracker ) {
			GUILayout.TextArea("Player " + player.gameObject.networkView.viewID);	
		}
		
		GUILayout.EndArea();
	}
}
