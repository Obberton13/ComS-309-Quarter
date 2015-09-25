using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

	[SerializeField]
	private float speed = 6.0F;

	[SerializeField]
	private float jump_force = 8.0F;

	[SerializeField]
	private float gravity = 20.0F;

	[SerializeField]
	private float rotate_speed = 45.0F;

	private CharacterController CharControl;
	private Vector3 moveDir;

	// Use this for initialization
	void Start () {
		CharControl = GetComponent<CharacterController>();
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
			if (Input.GetButton("Jump")) {
				moveDir.y = jump_force;
			}
		}

		moveDir.y -= gravity * Time.deltaTime;
		CharControl.Move(moveDir * Time.deltaTime);

		//Rotate the player
		transform.Rotate(Vector3.up * Time.deltaTime * Input.GetAxis("HorizontalJoy2") * rotate_speed);


		
	}
}
