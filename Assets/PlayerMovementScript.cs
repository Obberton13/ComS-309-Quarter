using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

	[SerializeField]
	private float speed = 6.0F;

	[SerializeField]
	private float jump_force = 8.0F;

	[SerializeField]
	private float gravity = 20.0F;

	private CharacterController CharControl;
	private Vector3 moveDir;

	// Use this for initialization
	void Start () {
		CharControl = GetComponent<CharacterController>();
		moveDir = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	


		if (CharControl.isGrounded) {
			moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDir = transform.TransformDirection(moveDir);
			moveDir *= speed;

			if (Input.GetButton("Jump")) {
				moveDir.y = jump_force;
			}
		}

		moveDir.y -= gravity * Time.deltaTime;
		CharControl.Move(moveDir * Time.deltaTime);



		
	}
}
