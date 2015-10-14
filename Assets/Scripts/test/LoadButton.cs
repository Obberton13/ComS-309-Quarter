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
			GameObject chunkObject = GameObject.Find("Main Camera");
			if(chunkObject == null)
			{
				print ("Loading failed");
				return;
			}
			print ("Loading...");
			List<ChunkInfo> infos;
			infos = my_save.load();
			chunkObject.GetComponent<World>().setChunkInfos(infos);
			print ("Loading complete");
			
		}
	}
}
