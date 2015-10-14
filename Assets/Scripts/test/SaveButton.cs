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
			GameObject chunkObject = GameObject.Find("Main Camera");
			if(chunkObject == null)
			{
				print ("It didn't work...");
				return;
			}
			print ("Saving...");
			World my_world = chunkObject.GetComponent<World>();
			my_save.save(my_world.getChunkInfos());
			print ("Saved.");
			}
		}
	}
