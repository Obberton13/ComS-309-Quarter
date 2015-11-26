using UnityEngine;
using System.Collections;

public class ChunkNetwork : MonoBehaviour {

	private ChunkInfo info;
	// Use this for initialization
	void Start () {
		info = GetComponent<Chunk>().getInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			//stream.SendNext(transform.position);
			//stream.SendNext(transform.rotation);
			stream.SendNext(info);
			
		}
		else
		{
			// Network player, receive data
			//this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			//this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

			this.info = (ChunkInfo)stream.ReceiveNext();
		}
	}
}
