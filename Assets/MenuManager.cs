using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public enum Buttons{
		newGame,
		loadGame,
		godMode,
		specMode,
		options
	};

	public Buttons highlighted;

	// Update is called once per frame
	public void highlightSelected (int selected) {
		Debug.Log("Shit fuck" + selected);
	}
	
	public void makeMenuSelection(int selection){
		Debug.Log("FROM: makeMenuSelection " + selection);
	}
}
