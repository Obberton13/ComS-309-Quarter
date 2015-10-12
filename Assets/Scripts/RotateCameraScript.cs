using UnityEngine;
using System.Collections;

public class RotateCameraScript : MonoBehaviour {

	[SerializeField]
	private float rotateSpeed = 45.0F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//TODO use this if you don't have an oculus. 
		transform.Rotate(Vector3.right * Time.deltaTime * Input.GetAxis("XboxRJoyVert") * rotateSpeed);


	}
}
