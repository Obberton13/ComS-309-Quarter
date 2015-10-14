using UnityEngine;
using System.Collections;

public class LoadButton : MonoBehaviour 
{
	
	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 150, 100, 30), "Load Game"))
		{
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			GameObject chunkObject = GameObject.Find("chunk");
			if(chunkObject == null)
			{
				return;
			}
			
			my_save.load(chunkObject.GetComponent<Chunk>().getInfo());
			
		}
	}
}
