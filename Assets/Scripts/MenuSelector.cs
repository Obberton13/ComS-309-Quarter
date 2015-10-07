using UnityEngine;
using System.Collections;

public class MenuSelector : MonoBehaviour {
	[SerializeField]
	private System.Collections.Generic.List<UnityEngine.UI.Button> buttons;
	int selected;

	// Use this for initialization
	void Start () {
		//buttons = new System.Collections.Generic.List<UnityEngine.UI.Button> ();
		selected = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//highlightSelected (selected);
	}

	void selectButton (int selected) {
		Debug.Log ("bastard " + selected);
	}

	void highlightSelected(int selected) {

	}
}
