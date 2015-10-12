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

	private GameObject PlayerCamera;

	//Crosshair varaibles;
	[SerializeField]
	private Texture2D crosshairTexture;
	private float crosshairScale = 0.35F;

	private RaycastHit crosshairHit; //information on what the player is looking at. 
	private const float DISTANCE_TO_HIT = 3.0F; //the distance that a player can place/pickup blocks.

	private const float CUBE_WIDTH = 1.0F;

	// Use this for initialization
	void Start () {
		CharControl = GetComponent<CharacterController>();
		PlayerCamera = transform.Find("OVRCameraRig").gameObject;
		//inventory = new PlayerInventoryScript();
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

		//Attempt to place an item
		if (Input.GetButtonDown("XboxRBumper")) {
		
			if (Physics.Raycast(transform.position, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
				print (crosshairHit.transform);
				Vector3 newLocation = crosshairHit.transform.position + crosshairHit.normal;
				print(newLocation);
				//checks if the space is open to place a block. 
				if (!Physics.CheckSphere(newLocation, CUBE_WIDTH * 0.49F)) { //if the cube with is 1, the radius is .49 so we can squeeze under the limit.
					//TODO remove from inventory! 
					Instantiate(basicBlock, newLocation, Quaternion.identity);
				}
			}

		}

		if (Input.GetButtonDown("XboxLBumper")) {
			if (Physics.Raycast(transform.position, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {

				//TODO add to inventory!
				Destroy(crosshairHit.transform.gameObject);
			}
		}
		
	}

	void OnGUI()
	{
		//Draw the crosshair at the center of the screen.
		GUI.DrawTexture(new Rect((Screen.width-crosshairTexture.width*crosshairScale)/2 ,(Screen.height-crosshairTexture.height*crosshairScale)/2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
	}
}
