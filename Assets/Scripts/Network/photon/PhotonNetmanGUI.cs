using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PhotonNetmanGUI : Photon.MonoBehaviour {
	
	public int guiSpace = 0;
	public string setting = "1";

	public void OnGUI()
    {
        GUILayout.Space(guiSpace);

        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
        {
            if (GUILayout.Button("Connect"))
            {
                PhotonNetwork.ConnectUsingSettings( setting );
            }
        }
        else
        {
            if (GUILayout.Button("Disconnect"))
            {
                PhotonNetwork.Disconnect();
            }
        }
    }
}
