using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {

	private int monstersLeft;

	[SerializeField]
	private GameObject monsterPref;

	private RaycastHit groundHit;

	private const int MIN_SPAWN_DISTANCE = 10; //monsters have to spawn at least this far away
	private const int MAX_SPAWN_DISTANCE = 50; //they can only spawn this far away though
	private const int MONSTERS_TO_SPAWN = 5; //number of monster to spawn around each player (so 2 * this will be the total number in a real game)

	// Use this for initialization
	void Start () {
		monstersLeft = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		//TODO put this in game Controller or something
		if (Input.GetKeyDown(KeyCode.Y) || Input.GetButtonDown("XboxY")) {
			Spawn();
		}


	}

	void Spawn() {


		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		//spawn a monster around each player
		foreach(GameObject player in allPlayers) {

			for (int i = 0; i < MONSTERS_TO_SPAWN; i++) {

				Vector3 randPos = Random.onUnitSphere * MIN_SPAWN_DISTANCE + Random.insideUnitSphere * (MAX_SPAWN_DISTANCE - MIN_SPAWN_DISTANCE);
				randPos.y = MAX_SPAWN_DISTANCE * 1.25F; //so the monster doesn't spawn underground
				Vector3 spawnPos = new Vector3(player.transform.position.x + randPos.x, player.transform.position.y + randPos.y, player.transform.position.z + randPos.z);
				GameObject newMon = PhotonNetwork.Instantiate("realMonsterPrefab", spawnPos, Quaternion.Euler(0, 0, 0), 0);
				monstersLeft++;
				//place monster on the ground after spawning
				if (Physics.Raycast(newMon.transform.position, Vector3.down, out groundHit, Mathf.Infinity)) {
					print("Replaced y: " + groundHit.point.y);
					newMon.transform.position = new Vector3(spawnPos.x, groundHit.point.y+1.5F, spawnPos.z);
				}
				else {
					//we're like ALL the way underground?
					//oh gosh I hope we aren't inside the ground
					PhotonNetwork.Destroy(newMon);
					//Destroy(newMon); //screw him then. 
					monstersLeft--;
				}

			}//end of for loop

		} //end of for each


	}
}
