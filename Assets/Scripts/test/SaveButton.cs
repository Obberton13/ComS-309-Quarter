using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour 
	{

	void OnGUI()
		{
		if (GUI.Button (new Rect (10, 100, 100, 30), "Save Game"))
			{
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			GameObject chunkObject = GameObject.Find("chunk");
			if(chunkObject == null)
			{
				return;
			}

			my_save.save(chunkObject.GetComponent<Chunk>().getInfo());

			}
		}
	}
