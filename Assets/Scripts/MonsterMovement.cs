using UnityEngine;
using System.Collections;

public class MonsterMovement : MonoBehaviour {

	private const float MONSTER_ATTACK_SPEED = 3.0F; //number of seconds the enemy "cooldown" is unitl it can attack again
	private const float MONSTER_ATTACK_RANGE = 2.0F; //number of units the enemies can attack in
	private const float MONSTER_MOVE_SPEED = 5.0F; //number of units the enemies move at per second
	
	private float canHit; //if canHit >= MONSTER_ATTACK_SPEED, the monster can attack.
	
	[SerializeField]
	private float jumpForce = 8.0F;
	
	[SerializeField]
	private float gravity = 20.0F;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	// Use this for initialization
	void Start () {
		
		canHit = MONSTER_ATTACK_SPEED;
		controller = GetComponent<CharacterController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject target = findClosestPlayer();
		
		
		//monster is out of attack range and needs to move closer somehow
		if ((target.transform.position - this.transform.position).sqrMagnitude >= MONSTER_ATTACK_RANGE * MONSTER_ATTACK_RANGE) {
			
			//keep eye contact to know that you mean business.
			transform.rotation = Quaternion.LookRotation(target.transform.position, Vector3.up);
			
			

			if (controller.isGrounded) {
				moveDirection = (target.transform.position - this.transform.position).normalized; //move towards them. You know you want to.
				moveDirection.y = 0;
				moveDirection *= MONSTER_MOVE_SPEED;
			}
			
			
			if (Physics.CheckSphere(this.transform.position + 1.0F * transform.forward.normalized, 0.49F)) {
				//There's a block in our way at our feet, so let's jump over it. 
				moveDirection.y = jumpForce; //TODO experiment with jumpForce to make it reasonable/good.
				
				//TODO use Physics.checkCapsule to see if we can fit into the area, if not we need to start breaking blocks.
				
			}
			

			moveDirection.y -= gravity * Time.deltaTime; //even monsters get gravity.
			controller.Move(moveDirection * Time.deltaTime); //begin the march.
			
			
		} //end of if out of range
		
		//monster is in range and will try to attack!
		else {
			
			if (canHit >= MONSTER_ATTACK_SPEED) {
				
				print ("OUCH");
				canHit = 0;
			}

			//makes it so the monster is never flying
			moveDirection = Vector3.zero;
			moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
		}


		//cooldown until can attack again
		canHit += Time.deltaTime; 
		
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
}
