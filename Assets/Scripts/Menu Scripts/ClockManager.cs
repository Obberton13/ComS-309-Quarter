using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour {

    public GameObject clock;

	// Use this for initialization
	void Start () {
        clock = GameObject.Find("Time");
	}
	
	// Update is called once per frame
	void Update () {
        //clock.GetComponent<Text>().text = System.DateTime.Now.ToShortTimeString();
	}
}
