using UnityEngine;
using System.Collections;

public class MonsterMovement : MonoBehaviour {

	private const float MONSTER_ATTACK_SPEED = 3.0F; //number of seconds the enemy "cooldown" is unitl it can attack again
	private const float MONSTER_ATTACK_RANGE = 2.0F; //number of units the enemies can attack in
	private const float MONSTER_MOVE_SPEED = 5.0F; //number of units the enemies move at per second
	
	private float canHit; //if canHit >= MONSTER_ATTACK_SPEED, the monster can attack.
	
	[SerializeField]
	private float jumpForce = 6.0F;
	
	[SerializeField]
	private float gravity = 20.0F;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;


	private RaycastHit monsterHit; //information on what the monster is hitting.




	// Use this for initialization
	void Start () {
		canHit = MONSTER_ATTACK_SPEED;
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {  




		GameObject target = findClosestPlayer();

		//monster is out of attack range and needs to move closer somehow
		//print((target.transform.position - this.transform.position).sqrMagnitude);
		if ((target.transform.position - this.transform.position).sqrMagnitude >= MONSTER_ATTACK_RANGE * MONSTER_ATTACK_RANGE) {
			

			
			//move forward by default
			if (controller.isGrounded) {
				moveDirection = (target.transform.position - this.transform.position).normalized; //move towards them. You know you want to.
				moveDirection.y = 0;
				moveDirection *= MONSTER_MOVE_SPEED;

				//keep eye contact to know that you mean business.
				//moved to ground because else we get weird things?
				transform.LookAt(target.transform.position);
			}
			

			//checks if there is a block directly in front of the monster at their feet
			if (Physics.CheckSphere((this.transform.position - new Vector3(0, .5F, 0)) + 1.01F * transform.forward.normalized, 0.49F)) {


				//checks if there is a block directly in front of the monster at their head
				if (Physics.CheckSphere((this.transform.position + new Vector3(0, .5F, 0)) + 1.01F * transform.forward.normalized, 0.49F)) {


					//now there are all 3 blocks in front of the monster! 
					if (Physics.CheckSphere((this.transform.position + new Vector3(0, 1.5F, 0)) + 1.01F * transform.forward.normalized, 0.49F)) {
						//case 3
						//delete cube above head this.transform.position + new Vector3(0, 1F, 0)) + 1.01F * transform.forward.normalized)
						print("3");
						if (Physics.Raycast(transform.position + new Vector3(0, 1.5F, 0), transform.forward, out monsterHit, 1.01F)) {
							if (!monsterHit.transform.GetComponent<MeshCollider>())
							{
								if (canHit >= MONSTER_ATTACK_SPEED) {
									attackBlock(monsterHit.transform.gameObject);
									print("OUCH3");
									canHit = 0;
								}
							}
						}
					
					}

					else {
						//case 2 on your cheat sheet ryan
						//break the cube at the head (this.transform.position + new Vector3(0, .5F, 0)) + 1.01F * transform.forward.normalized)
						print("2");
						if (Physics.Raycast(transform.position + new Vector3(0, 0.5F, 0), transform.forward, out monsterHit, 1.01F)) {
							if (!monsterHit.transform.GetComponent<MeshCollider>())
							{
								if (canHit >= MONSTER_ATTACK_SPEED) {
									attackBlock(monsterHit.transform.gameObject);
									print("OUCH2");
									canHit = 0;
								}
							}
						}
					}




				}
				else {

					//there is a block at feet and above head, so not enough room to jump through to.
					if (Physics.CheckSphere((this.transform.position + new Vector3(0, 1.5F, 0)) + 1.01F * transform.forward.normalized, 0.49F)) {
						//case 4 ryan
						// break the block at (this.transform.position + new Vector3(0, 1F, 0)) + 1.01F * transform.forward.normalized)
						print("4");
						if (Physics.Raycast(transform.position + new Vector3(0, 1.5F, 0), transform.forward, out monsterHit, 1.01F)) {
							if (!monsterHit.transform.GetComponent<MeshCollider>())
							{
								if (canHit >= MONSTER_ATTACK_SPEED) {
									attackBlock(monsterHit.transform.gameObject);
									print("OUCH4");
									canHit = 0;
								}
							}
						}
					}

					else {
						print("1");
						//we don't want to inifinite jump
						if (controller.isGrounded) {
							moveDirection.y = jumpForce;
						}	
					}
				}
				
			}

			//there is a block at head level and above head level (not feet level)
			else if (Physics.CheckSphere((this.transform.position + new Vector3(0, .5F, 0)) + 1.01F * transform.forward.normalized, 0.49F)) {
				if (Physics.CheckSphere((this.transform.position + new Vector3(0, 1.5F, 0)) + 1.01F * transform.forward.normalized, 0.49F)) {
					//case 6
					//(this.transform.position + new Vector3(0, .5F, 0)) + 1.01F * transform.forward.normalized)
					print("6");
					if (Physics.Raycast(transform.position + new Vector3(0, 1.5F, 0), transform.forward, out monsterHit, 1.01F)) {
						if (!monsterHit.transform.GetComponent<MeshCollider>())
						{
							if (canHit >= MONSTER_ATTACK_SPEED) {
								attackBlock(monsterHit.transform.gameObject);
								print("OUCH6");
								canHit = 0;
							}
						}
					}
				}
				else {
					//case 5
					print("5");
					if (Physics.Raycast(transform.position + new Vector3(0, 1.5F, 0), transform.forward, out monsterHit, 1.01F)) {
						if (!monsterHit.transform.GetComponent<MeshCollider>())	{
							if (canHit >= MONSTER_ATTACK_SPEED) {
								attackBlock(monsterHit.transform.gameObject);
								print("OUCH5");
								canHit = 0;
							}
						}
					}
				}
			}

			moveDirection.y -= gravity * Time.deltaTime; //even monsters get gravity.
			controller.Move(moveDirection * Time.deltaTime); //begin the march.
			
			
		} //end of if out of range
		
		//monster is in range and will try to attack!
		else {

			//print("GONNA GETCHA");
			if (canHit >= MONSTER_ATTACK_SPEED) {
				
				print("OUCH");
				canHit = 0;
			}

			//makes it so the monster is never flying
			moveDirection = Vector3.zero;
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
		}


		//cooldown until can attack again
		canHit += Time.deltaTime; 


		//TODO in case the monster falls of the stage.
		//if (transform.position.y < -100) {
		//	Destroy(this.gameObject);
		//}


	} //end of Update()
	
	
	//Finds the closest player in the game
	private GameObject findClosestPlayer() {
		
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
		GameObject closestPlayer = this.gameObject; //will stay still if there are no players
		float distance = -1.0F;
		
		foreach(GameObject player in allPlayers) {
			float tempDist = (player.transform.position - this.transform.position).sqrMagnitude;
			if (distance == -1.0F || tempDist < distance) {
				closestPlayer = player;
				distance = tempDist;
			}
		}
		
		return closestPlayer;
		
		
	} //end of findClosestPlayer()

	private void attackBlock(GameObject block) {

		//TODO make it so monsters can only destroy blocks
		//if (block.tag.Equals("Block")) {
			//99% chance to destroy right now
			//TODO change chance based off of block info.
			if (Random.Range(0.0F, 1.0F) <= .99F) {
				Destroy(block); //TODO
				//Remove from Chunk
			}
		//}

	}


	//Testing for monster collision
	void OnDrawGizmos() {
		Gizmos.DrawSphere(this.transform.position - new Vector3(0, .5F, 0) + 1.01F * transform.forward.normalized, 0.49F); //bottom block in front
		Gizmos.DrawSphere(this.transform.position + new Vector3(0, .5F, 0) + 1.01F * transform.forward.normalized, 0.49F); //bottom block in front
		Gizmos.DrawSphere(this.transform.position + new Vector3(0, 1.5F, 0) + 1.01F * transform.forward.normalized, 0.49F); //bottom block in front
	}

	//Testing for monster attack speed
	void OnGUI() {
		//GUI.Label(new Rect(10, 10, 100, 20), "canHit: " + canHit);
	}
}
