using UnityEngine;
using System.Collections;

public class RoomSetup : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	void OnJoinedRoom()
	{
		GameObject player = PhotonNetwork.Instantiate("playerPrefab", new Vector3(-150, 79, -150), Quaternion.identity, 0);
		//incase we wanna do stuff with the player.
	}

}
