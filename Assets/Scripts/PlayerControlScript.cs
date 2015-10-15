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
	private float rotateSpeed = 90.0F;

	[SerializeField]
	private GameObject basicBlock;
    
    private World _world;

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
        _world = GameObject.Find("Main Camera").GetComponent<World>();
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
                //print (crosshairHit.point);

                Debug.Log(crosshairHit.point);

                int tempX = Mathf.FloorToInt(crosshairHit.point.x + crosshairHit.normal.x / 2f);
                int tempY = Mathf.FloorToInt(crosshairHit.point.y + crosshairHit.normal.y / 2f);
                int tempZ = Mathf.FloorToInt(crosshairHit.point.z + crosshairHit.normal.z / 2f);

				//tempX = (int) Mathf.FloorToInt(tempX); //I have a feeling this was giving a floating point error that made this cast to different values.
				//tempY = (int) Mathf.FloorToInt(tempY); //That is why sometimes Ryan's thing was spawning blocks underground.
				//tempZ = (int) Mathf.FloorToInt(tempZ);

				//tempX += 0.5F;
				//tempY += 0.5F; //cause YOLO
				//tempZ += 0.5F; 

				//if (crosshairHit.normal.y == 1) {
				//	tempY -= 1.0F; //math is weird. 
				//}

				//if (crosshairHit.normal.x == 1) {
				//	tempX -= 1.0F; //math is still weird...
				//}

    //            if(crosshairHit.normal.z == 1)
    //            {
    //                tempZ -= 1.0F; //math is definitely not weird at all. This makes perfect sense if you think about it.
    //            }

				Vector3 newLocation = new Vector3(tempX, tempY, tempZ);
				//print(newLocation);

				//checks if the space is open to place a block.
				if (!Physics.CheckSphere(newLocation + new Vector3(.5f, .5f, .5f), CUBE_WIDTH * 0.49F)) { //if the cube with is 1, the radius is .49 so we can squeeze under the limit.

                    //TODO remove from inventory! 
                    //TODO place the object in the chunk instead of instantiating
                    _world.putBlock(tempX, tempY, tempZ, 1);
                    //Debug.Log(newLocation);
					//Instantiate(basicBlock, newLocation, Quaternion.identity);
				}
			}

		}

		if (Input.GetButtonDown("XboxLBumper")) {
			if (Physics.Raycast(transform.position, PlayerCamera.transform.forward, out crosshairHit, DISTANCE_TO_HIT)) {
                if (!crosshairHit.transform.GetComponent<MeshCollider>())
                {
				    Destroy(crosshairHit.transform.gameObject);
                }
                //TODO remove things from the map.
                //TODO add to inventory!
            }
		}
		
	}

    void OnGUI()
    {
        //Draw the crosshair at the center of the screen.
        GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
    }
}
