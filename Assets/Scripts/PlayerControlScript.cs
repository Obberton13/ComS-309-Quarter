using UnityEngine;
using System.Collections;

public class PlayerControlScript : Photon.PunBehaviour {
	
	[SerializeField]
	private float speed = 6.0F;

	[SerializeField]
	private float jumpForce = 8.0F;

	[SerializeField]
	private float gravity = 20.0F;

	[SerializeField]
	private float rotateSpeed = 90.0F;

	[SerializeField]
	private GameObject basicBlock;

	private CharacterController CharControl;
	private Vector3 moveDir;
	private PlayerInventoryScript inventory;

	private GameObject PlayerCamera;
	private GameObject PlayerSword;

	//Crosshair varaibles;
	[SerializeField]
	private Texture2D crosshairTexture;
	private float crosshairScale = 0.35F;

	private RaycastHit crosshairHit; //information on what the player is looking at. 
	private const float DISTANCE_TO_HIT = 3.0F; //the distance that a player can place/pickup blocks.

	private const float CUBE_WIDTH = 1.0F;

	public MenuState ms;

	private bool canKill = false;

	private int health;

	// Use this for initialization
	void Start () {
		CharControl = GetComponent<CharacterController>();

		transform.Find("OVRCameraRig").GetComponent<OVRManager>().resetTrackerOnLoad = true;

		PlayerCamera = transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").gameObject;
		//PlayerCamera = transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").Find("Sword").gameObject;
		print (PlayerCamera.name);
		PlayerSword = transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").Find("Sword").gameObject;

		//inventory = new PlayerInventoryScript();
		moveDir = Vector3.zero;
		ms = GameObject.Find ("Game Controller").GetComponent<MenuState>();
		health = 100;

	}

	[PunRPC]
	void place_block_rpc(int x, int y, int z, byte type)
	{
		World world;
		world = GameObject.Find("Game Controller").GetComponent<World>(); 
		world.putBlock( x, y, z, type );

	}

	
	// Update is called once per frame
	void Update () {
	
		//if(ms.menuState == MenuState.MenuStates.playerPlaying){
		if (true) { //TODO
			//Move the camera in the X/Y direction
			if (CharControl.isGrounded) {
				moveDir = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				moveDir = transform.TransformDirection (moveDir);
				moveDir *= speed;

				//move "up"
				if (Input.GetButton ("XboxA")) {
					moveDir.y = jumpForce;
				}
			}

			moveDir.y -= gravity * Time.deltaTime;
			CharControl.Move (moveDir * Time.deltaTime);

			//Rotate the player
			transform.Rotate (Vector3.up * Time.deltaTime * Input.GetAxis ("XboxRJoyHoriz") * rotateSpeed);

			Debug.DrawLine(transform.position, transform.position + PlayerCamera.transform.forward*DISTANCE_TO_HIT);
		//Attempt to place an item
		if (Input.GetButtonDown("XboxRBumper")) {
		

			if (Physics.Raycast(transform.position, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
                //print (crosshairHit.point);

                //Debug.Log(crosshairHit.point);

					float tempX = crosshairHit.point.x + crosshairHit.normal.x;
					float tempY = crosshairHit.point.y + crosshairHit.normal.y;
					float tempZ = crosshairHit.point.z + crosshairHit.normal.z;

					tempX = Mathf.FloorToInt(tempX);
					tempY = Mathf.FloorToInt(tempY);
					tempZ = Mathf.FloorToInt(tempZ);

					tempX += 0.5F;
					tempY += 0.5F; //cause YOLO
					tempZ += 0.5F;

					if (crosshairHit.normal.y == 1) {
						tempY -= 1.0F; //math is weird. 
					}

					if (crosshairHit.normal.x == 1) {
						tempX -= 1.0F; //math is still weird...
					}

					Vector3 newLocation = new Vector3 (tempX, tempY, tempZ);
					print(newLocation);

					//checks if the space is open to place a block. 
					if (!Physics.CheckSphere (newLocation, CUBE_WIDTH * 0.49F)) { //if the cube with is 1, the radius is .49 so we can squeeze under the limit.
						//TODO remove from inventory! 
						print ( newLocation );
						photonView.RPC("place_block_rpc", PhotonTargets.All, Mathf.FloorToInt (tempX), Mathf.FloorToInt( tempY ), Mathf.FloorToInt( tempZ ), (byte)1 );
					}
				}

			}
		if (Input.GetButtonDown("XboxLBumper")) {
			if (Physics.Raycast(transform.position, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
                if (crosshairHit.transform.GetComponent<MeshCollider>())
                {
				    //Destroy(crosshairHit.transform.gameObject);
						//photonView.RPC("putBlockHelper", PhotonTargets.All, crosshairHit.transform.position.x, crosshairHit.transform.position.y, crosshairHit.transform.position.z, 0 );
						//_world.putBlock( (int)crosshairHit.transform.position.x, (int)crosshairHit.transform.position.y, (int)crosshairHit.transform.position.z, 0 );

						int tempX = Mathf.FloorToInt(crosshairHit.point.x - crosshairHit.normal.x / 2f);
						int tempY = Mathf.FloorToInt(crosshairHit.point.y - crosshairHit.normal.y / 2f);
						int tempZ = Mathf.FloorToInt(crosshairHit.point.z - crosshairHit.normal.z / 2f);

						photonView.RPC("place_block_rpc", PhotonTargets.All, tempX, tempY, tempZ, (byte)0 );
					}
                //TODO remove things from the map.
                //TODO add to inventory!
            }
		}
	
		if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.U)) {
				//Do the sword animation
				if(PlayerSword.GetComponent<SwingSword>().attack()) {
					canKill = true;
				}

		}

		if (Input.GetButtonDown("XboxBack")) {
				OVRManager.display.RecenterPose();
		}

		if (PlayerSword.GetComponent<SwingSword>().getEndSwing()) {
			//we can check if there is a monster that was just killed. Only kills one at a time. 
			if (canKill) {
				//if the monster is directly in front of the player
				if (Physics.Raycast(transform.position, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
					if (crosshairHit.transform.gameObject.tag == "Monster") {
						PhotonNetwork.Destroy(crosshairHit.transform.gameObject);
						//Destroy(crosshairHit.transform.gameObject);
					}
					canKill = false;
				}
				//if the monster is just a little bit to the right?
				else if (Physics.Raycast(transform.position + 0.75F*transform.right, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
					if (crosshairHit.transform.gameObject.tag == "Monster") {
						//Destroy(crosshairHit.transform.gameObject);
						PhotonNetwork.Destroy(crosshairHit.transform.gameObject);
					}
					canKill = false;
				}
				//if the monster is just a little bit to the left?
				else if (Physics.Raycast(transform.position - 0.75F*transform.right, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
					if (crosshairHit.transform.gameObject.tag == "Monster") {
						//Destroy(crosshairHit.transform.gameObject);
						PhotonNetwork.Destroy(crosshairHit.transform.gameObject);
					}
					canKill = false;
				}
			}
		}

	}
	}

	public void loseHealth(int lost) {
		health -= lost;

		if (health <= 0) {
			//DIE
		}

	}


    void OnGUI()
    {
        //Draw the crosshair at the center of the screen.
        GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
		GUI.Label(new Rect(10, 10, 250, 40), "Health Remaining: " + health);
    }
}
