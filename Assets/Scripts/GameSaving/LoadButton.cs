using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadButton : MonoBehaviour 
{
	
	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 150, 100, 30), "Load Game"))
		{
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			my_save.load();
		}
	}

	public void loadGame()
	{
		SaveWorld my_save = this.GetComponent<SaveWorld>();
		my_save.load();
	}
}
