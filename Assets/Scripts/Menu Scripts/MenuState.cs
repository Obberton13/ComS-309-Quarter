using UnityEngine;
using System.Collections;

public class MenuState : MonoBehaviour {

	public enum MenuStates{
		_START,
		inMainMenu,
		playerPlaying,
		_END
	};

	public MenuStates menuState = MenuStates.inMainMenu;
}
