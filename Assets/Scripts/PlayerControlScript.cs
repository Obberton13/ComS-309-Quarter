using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

	[SerializeField]
	private float speed = 6.0F;

	[SerializeField]
	private float jumpForce = 8.0F;

	[SerializeField]
	private float gravity = 20.0F;

	[SerializeField]
	private float rotateSpeed = 45.0F;

	[SerializeField]
	private GameObject basicBlock;

	private CharacterController CharControl;
	private Vector3 moveDir;
	private PlayerInventoryScript inventory;

	private const float CUBE_WIDTH = 1.0F;

	// Use this for initialization
	void Start () {
		CharControl = GetComponent<CharacterController>();
		inventory = new PlayerInventoryScript();
		moveDir = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	

		//Move the camera in the X/Y direction
		if (CharControl.isGrounded) {
			moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDir = transform.TransformDirection(moveDir);
			moveDir *= speed;

			//move "up"
			if (Input.GetButton("XboxA")) {
				moveDir.y = jumpForce;
			}
		}

		moveDir.y -= gravity * Time.deltaTime;
		CharControl.Move(moveDir * Time.deltaTime);

		//Rotate the player
		transform.Rotate(Vector3.up * Time.deltaTime * Input.GetAxis("XboxRJoyHoriz") * rotateSpeed);

		//print(Input.GetAxis("XboxDpadHoriz") + ", " + Input.GetAxis("XboxDpadVert"));

		//Attempt to place an item
		if (Input.GetButtonDown("XboxRBumper")) {

			//get the exact new location for the block to be placed
			//TODO this doesn't take into account the rotation of the players head? 
			float tempX = transform.position.x + transform.forward.x;
			float tempY = transform.position.y + transform.forward.y;
			float tempZ = transform.position.z + transform.forward.z;
		
			//normalize to a grid size.
			tempX = (int) (Mathf.Round(tempX / CUBE_WIDTH) * CUBE_WIDTH);
			tempY = (int) (Mathf.Round(tempY / CUBE_WIDTH) * CUBE_WIDTH);
			tempZ = (int) (Mathf.Round(tempZ / CUBE_WIDTH) * CUBE_WIDTH);

			//Or else the block is half in the ground
			tempY -= 0.5F; //this weird by the way. 

			//save it all to a vector3location
			Vector3 blockLocation = new Vector3(tempX, tempY, tempZ);

			//TODO check if there is already something there
			//TODO add it to the game memory somehow
			Instantiate(basicBlock, blockLocation, Quaternion.identity);


		}

		
	}
}
