using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveWorld : MonoBehaviour
	{

	// Will not work for web, but all other platforms
	public void save( ChunkInfo to_save )
		{
		BinaryFormatter my_bf = new BinaryFormatter();
		FileStream my_file = File.Create( Application.persistentDataPath + "/saveFile.dat" );
		my_bf.Serialize( my_file, to_save );
		my_file.Close();
		}

	public void load( ChunkInfo to_load )
		{
		if (File.Exists( Application.persistentDataPath + "/saveFile.dat" ) )
			{
			BinaryFormatter my_bf = new BinaryFormatter();
			FileStream my_file = File.Open( Application.persistentDataPath + "/saveFile.dat", FileMode.Open );
			to_load = (ChunkInfo)my_bf.Deserialize( my_file );
			my_file.Close ();
			}
		}
	}
