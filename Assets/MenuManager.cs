using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public enum Buttons{
		_START,
		newGame,
		loadGame,
		godMode,
		specMode,
		options,
		_END
	};

    public bool mousedOver;

	public Buttons selected;

	// Use this for initialization
	void Start () {
		selected = Buttons.newGame;
		highlightSelected (selected);
        mousedOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("up")) {
			unhighlightSelected (selected);
			--selected;
			checkSelection();
			highlightSelected (selected);
		}
		if (Input.GetKeyDown ("down")) {
			unhighlightSelected (selected);
			++selected;
			checkSelection();
			highlightSelected (selected);
		}
        if (mousedOver)
        {
            unhighlightSelected(selected);
            highlightSelected(selected);
            mousedOver = false;
        }
	}

	public void highlightSelected (Buttons selected) {
		//Debug.Log("Highlighting: btn_" + selected);
		GameObject button = GameObject.Find ("btn_" + selected);
		button.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300, 33);
	}

	public void unhighlightSelected (Buttons selected) {
		//Debug.Log("Unhighlighting: btn_" + selected);
		GameObject button = GameObject.Find ("btn_" + selected);
		button.GetComponent<RectTransform> ().sizeDelta = new Vector2 (220, 33);
	}
	
	public void makeMenuSelection(int selection){
		Debug.Log("FROM: makeMenuSelection " + selection);
        Debug.Log("Time: " + System.DateTime.Now);
	}

	public void checkSelection(){
		if (selected == Buttons._START) {
			selected = Buttons._END - 1;
		} else if (selected == Buttons._END) {
			selected = Buttons._START + 1;
		}
	}
}
