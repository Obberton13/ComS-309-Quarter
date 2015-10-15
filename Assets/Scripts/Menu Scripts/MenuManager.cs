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
    public Buttons oldVal;

	void Start () {
		selected = Buttons.newGame;
        oldVal = selected;
		highlightSelected (selected);
        mousedOver = false;
	}
	
	void Update () {

        // Checks if we need to update the currently selected on on screen
        // This is merely updating the view
        if(selected != oldVal)
        {
            unhighlightSelected(oldVal);
            highlightSelected(selected);
            oldVal = selected;
        }

        // Checking for input
        if (Input.GetKeyDown ("up")) {
			--selected;
			checkSelection();
		}
		else if (Input.GetKeyDown ("down")) {
			++selected;
			checkSelection();
		}
		else if (Input.GetKeyDown("escape"))
		{
			GameObject menu = GameObject.Find ("MainMenu");
			menu.GetComponent<Canvas>().enabled = true;
		}
	}

	public void highlightSelected (Buttons selected) {
		GameObject button = GameObject.Find ("btn_" + selected);
		button.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300, 33);
	}

	public void unhighlightSelected (Buttons selected) {
		GameObject button = GameObject.Find ("btn_" + selected);
		button.GetComponent<RectTransform> ().sizeDelta = new Vector2 (220, 33);
	}
	
	public void makeMenuSelection(int selection){
		Debug.Log("FROM: makeMenuSelection " + selection);

		switch (selection) {
		case 0:
			GameObject menu = GameObject.Find ("MainMenu");
			menu.GetComponent<Canvas>().enabled = false;
			break;
		case 1:
			GameObject.Find ("SaveObject").GetComponent<SaveWorld>().load();
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		default:
			break;
		}
	}

	public void checkSelection(){
		if (selected == Buttons._START) {
			selected = Buttons._END - 1;
		} else if (selected == Buttons._END) {
			selected = Buttons._START + 1;
		}
	}

    public void hoverSelection(int button)
    {
       //Debug.Log("This is a message from mouseEnter");
       selected = (Buttons) button + 1;
    }
}
