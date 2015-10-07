using UnityEngine;
using System.Collections;

public class changeSelected : MonoBehaviour {

	// Update is called once per frame
	public void highlightSelected (int selected) {
		Debug.Log("Shit fuck" + selected);
	}

	public void makeMenuSelection(int selection){
		Debug.Log("FROM: makeMenuSelection " + selection);
	}
}
