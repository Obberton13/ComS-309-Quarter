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

		//only the first player in the room needs to generate a world
		//JK NEVERMIND
		//if (PhotonNetwork.playerList.Length == 1)
		//{
		//	GameObject.Find("Game Controller").GetComponent<World>().enabled = true;
		//}

		//incase we wanna do stuff with the player. We do. See below.
		//TODO change spawn point vector3.
		GameObject player = PhotonNetwork.Instantiate("playerPrefab", new Vector3(-150, 79, -150), Quaternion.identity, 0);

		//we should only be able to control our own player! 
		player.GetComponent<PlayerControlScript>().enabled = true;

		//only one camera and audio listener in the local scene should be on.
		player.GetComponentInChildren<Camera>().enabled = true;
		player.GetComponentInChildren<AudioListener>().enabled = true;


	}

}
