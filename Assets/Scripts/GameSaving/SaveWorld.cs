using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveWorld : MonoBehaviour
	{

	// Will not work for web, but all other platforms
	public void save( List<ChunkInfo> to_save )
		{
		BinaryFormatter my_bf = new BinaryFormatter();
		FileStream my_file = File.Create( Application.persistentDataPath + "/saveFile.dat" );
		my_bf.Serialize( my_file, to_save );
		my_file.Close();
		}

	public List<ChunkInfo> load( )
		{
		if (File.Exists( Application.persistentDataPath + "/saveFile.dat" ) )
			{
			List<ChunkInfo> to_return;
			BinaryFormatter my_bf = new BinaryFormatter();
			FileStream my_file = File.Open( Application.persistentDataPath + "/saveFile.dat", FileMode.Open );
			to_return = (List<ChunkInfo>)my_bf.Deserialize( my_file );
			my_file.Close ();
			return to_return;
			}
		return null;
		}
	}

