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

		if (PhotonNetwork.playerList.Length == 1)
		{
			GameObject.Find("Game Controller").GetComponent<World>().enabled = true;
		}

		GameObject player = PhotonNetwork.Instantiate("playerPrefab2", new Vector3(-150, 79, -150), Quaternion.identity, 0);
		//incase we wanna do stuff with the player.
		player.GetComponent<PlayerControlScript>().enabled = true;
		GameObject PlayerCamera = transform.Find("CenterEyeAnchor").gameObject;
		PlayerCamera.GetComponent<Camera>().enabled = true;
		PlayerCamera.GetComponent<AudioListener>().enabled = true;


	}

}
