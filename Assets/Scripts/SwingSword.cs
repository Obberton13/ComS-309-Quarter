using UnityEngine;
using System.Collections;

public class SwingSword : MonoBehaviour {

	private float startXAngle = 90.0F;
	private float endXAngle = 50.0F;

	private float startXPos = 0.42F;
	private float endXPos = 0F;

	private bool startSwing = false;
	private float startTime = 0.0F;
	private bool endSwing = false;
	private float endTime = 0.0F;

	private const float SWING_TIME = 0.3F; //The time it takes to complete half a swing. 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//testing
		/*if (Input.GetKeyDown(KeyCode.U)) {
			if (!endSwing && !startSwing) {
				startSwing = true;
			}
		}
		*/

		if (startSwing) {
			float angle = Mathf.Lerp(startXAngle, endXAngle, startTime / SWING_TIME);
			float pos = Mathf.Lerp(startXPos, endXPos, startTime / SWING_TIME);
			startTime+=Time.deltaTime;
			transform.localEulerAngles = new Vector3(angle, 90, 2*(90-angle));
			transform.localPosition = new Vector3(pos, -0.09F, 0.31F);
			if (startTime >= SWING_TIME) {
				startSwing = false;
				endSwing = true;
				startTime = 0.0F;
			}
		}

		if (endSwing) {
			float angle = Mathf.Lerp(endXAngle, startXAngle, endTime / SWING_TIME);
			float pos = Mathf.Lerp(endXPos, startXPos, endTime / SWING_TIME);
			endTime+=Time.deltaTime;
			transform.localEulerAngles = new Vector3(angle, 90, 2*(90-angle));
			transform.localPosition = new Vector3(pos, -0.09F, 0.31F);
			if (endTime >= SWING_TIME) {
				endSwing = false;
				endTime = 0.0F;
			}
		}



	}

	//if this returns true then we know we just attacked and maybe hit something.
	public bool getEndSwing() {
		return endSwing;
	}

	public bool attack() {
		if (!endSwing && !startSwing) {
			startSwing = true;
			return true;
		}
		return false;
	}
}
