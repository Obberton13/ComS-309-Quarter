using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveButton : MonoBehaviour 
	{

	void OnGUI()
		{
		if (GUI.Button (new Rect (10, 100, 100, 30), "Save Game"))
			{
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			my_save.save();
			}
		}
	}
