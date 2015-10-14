using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveButton : MonoBehaviour 
	{

	[SerializeField]
	private GameObject _world;

	void OnGUI()
		{
		if (GUI.Button (new Rect (10, 100, 100, 30), "Save Game"))
			{
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			print ("Saving...");
			World my_world = _world.GetComponent<World>();
			my_save.save(my_world.getChunkInfos());
			print ("Saved.");
			}
		}
	}
