using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadButton : MonoBehaviour 
{

	[SerializeField]
	private GameObject _world;
	
	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 150, 100, 30), "Load Game"))
		{
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			print ("Loading...");
			List<ChunkInfo> infos;
			infos = my_save.load();
			World my_world = _world.GetComponent<World>();
			my_world.setChunkInfos(infos);
			print ("Loading complete");
			
		}
	}
}
