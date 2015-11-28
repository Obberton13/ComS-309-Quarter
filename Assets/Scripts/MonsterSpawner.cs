using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {

	private int monstersLeft;

	[SerializeField]
	private GameObject monsterPref;

	// Use this for initialization
	void Start () {
		monstersLeft = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.P)) {
			Spawn();
		}


	}

	void Spawn() {


		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

		//spawn 5 around each player
		foreach(GameObject player in allPlayers) {

			Vector3 randPos = Random.insideUnitSphere * 10;
			randPos.y += 10;

			//Vector3 spawnPos = new Vector3(player.transform.position.x + randPos.x, player.transform.position.y + randPos.y, player.transform.position.z + randPos.z);
			Vector3 spawnPos = new Vector3(player.transform.position.x + randPos.x, 1, player.transform.position.z + randPos.z);

			Instantiate(monsterPref, spawnPos, Quaternion.Euler(0, 0, 0));


		}





	}
}
