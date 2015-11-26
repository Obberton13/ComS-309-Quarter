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

		// Setup our File and Binary Formatter
		BinaryFormatter my_bf = new BinaryFormatter();
		FileStream my_file = File.Create( Application.persistentDataPath + "/saveFile.dat" );

		// Gain access to our World and Player objects
		World my_world = _world.GetComponent<World>();
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		// Create our new SaveFile object
		SaveFile save = new SaveFile (my_world.getChunkInfos (), player.transform.position);

		// Write our SaveFile object to disk
		my_bf.Serialize( my_file, save );
		my_file.Close();

		print ("Saved.");
		}

	public void load( )
		{

		// Check to see if a previous save_file exists
		if (File.Exists (Application.persistentDataPath + "/saveFile.dat")) {

			print ("Loading...");

			// Setup our File and Binary Formatter
			BinaryFormatter my_bf = new BinaryFormatter ();
			FileStream my_file = File.Open (Application.persistentDataPath + "/saveFile.dat", FileMode.Open);

			// Gain access to our World and Player objects
			World my_world = _world.GetComponent<World> ();
			GameObject player = GameObject.FindGameObjectWithTag ("Player");

			// Retrieve our old SaveFile data
			SaveFile save = (SaveFile)my_bf.Deserialize (my_file);
			List<ChunkInfo> infos = save.getInfos ();
			my_file.Close ();

			// Set Player position
			Vector3 player_position = save.getPlayerPosition ();
			player_position.y += 5;
			player.transform.position = player_position;

			// Clear and set World info
			//my_world.clearChunkInfos ();
			my_world.setChunkInfos (infos);

			print ("Loading complete");

		} else {
			print ( "No existing Save File.");
		}
	}
	[Serializable]
	class SaveFile
	{
		private List<ChunkInfo> infos;
		private float player_x;
		private float player_y;
		private float player_z;

		public SaveFile()
		{
			infos = null;
			player_x = 0;
			player_y = 100;
			player_z = 0;
		}

		public SaveFile(List<ChunkInfo> infos, Vector3 player_position)
		{
			this.infos = infos;
			this.player_x = player_position.x;
			this.player_y = player_position.y;
			this.player_z = player_position.z;
		}

		public List<ChunkInfo> getInfos()
		{
			return infos;
		}

		public Vector3 getPlayerPosition()
		{
			return new Vector3(player_x, player_y, player_z);
		}
	}
}

