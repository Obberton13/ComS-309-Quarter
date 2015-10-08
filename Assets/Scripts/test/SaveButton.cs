using UnityEngine;
using System.Collections;

public class SaveButton : MonoBehaviour 
	{

	void OnGUI()
		{
		if (GUI.Button (new Rect (10, 100, 100, 30), "Save Game"))
			{
<<<<<<< HEAD
			SaveWorld my_save = this.GetComponent<SaveWorld>();
			my_save.save ( );

=======
			//SaveO.save( info );
>>>>>>> 617f4c2f621ba284b4088c46d837aacec58e1011
			}
		}
	}
