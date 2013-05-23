using System;
using UnityEngine;
using System.Collections;

public class PhotonNetman : Photon.MonoBehaviour {
	
	public GameObject playerPrefab;
	
	private GameObject spawnPoint;
	
	// Use this for initialization
    public virtual void Start ()
    {
		// Call OnConnectedToMaster() instead of auto join a the lobby
        PhotonNetwork.autoJoinLobby = false;
    }
	
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: (null, true, true, 4);");
        PhotonNetwork.CreateRoom(null, true, true, 4);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		// Find a free spawn point for my player
		GameObject[] gos = GameObject.FindGameObjectsWithTag("SpawnPoint");
		foreach( GameObject spawngo in gos) {
			if( spawngo.GetComponent<RespawnPoint>().free ) {
				spawnPoint = spawngo;
				spawngo.GetComponent<RespawnPoint>().free = false;
				spawngo.GetComponent<RespawnPoint>().player = PhotonNetwork.player;
				break;
			}
		}
		
		if( spawnPoint == null ) 
			Debug.Log("No free spawnpoint found!");
		
		// Spawn my player
		Debug.Log("Intatiate player at " + spawnPoint.transform.position);
		GameObject handle = PhotonNetwork.Instantiate(playerPrefab.name,spawnPoint.transform.position,Quaternion.identity,0);
		PlayerManager pman = handle.gameObject.GetComponent<PlayerManager>();
		pman.SendMessage("SetPlayer",PhotonNetwork.player);
    }
	
}
