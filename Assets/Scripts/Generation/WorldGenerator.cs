using UnityEngine;
using System.Collections;

public class WorldGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Random.seed = 1;
		Noise.init();
		Debug.Log(Noise.getNoiseValue(15f, 16f, 17f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
