using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveWorld : MonoBehaviour
	{
	[SerializeField]
	private GameObject _world;

	// Will not work for web, but all other platforms
	public void save( )
		{
		print ("Saving...");
		BinaryFormatter my_bf = new BinaryFormatter();
		FileStream my_file = File.Create( Application.persistentDataPath + "/saveFile.dat" );
		World my_world = _world.GetComponent<World>();
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		my_bf.Serialize( my_file, my_world.getChunkInfos() );
		my_file.Close();
		print ("Saved.");
		}

	public void load( )
		{
		if (File.Exists( Application.persistentDataPath + "/saveFile.dat" ) )
			{
			print ("Loading...");
			BinaryFormatter my_bf = new BinaryFormatter();
			FileStream my_file = File.Open( Application.persistentDataPath + "/saveFile.dat", FileMode.Open );
			List<ChunkInfo> infos = (List<ChunkInfo>)my_bf.Deserialize( my_file );
			World my_world = _world.GetComponent<World>();

			my_world.setChunkInfos(infos);
			my_file.Close ();
			print ("Loading complete");
			}
		}
	}

