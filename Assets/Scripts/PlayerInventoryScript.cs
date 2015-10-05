using UnityEngine;
using System.Collections;

public class PlayerInventoryScript {


	private const int Empty = 0;
	private const int GrassBlock = 1;
	private const int WaterBlock = 2;
	//private const int AirBlock = 3; //TBA



	//TODO Make this not an int. Cause you know. You can't save to and int. 
	//Well you could.... but that would be incredibly stupid. 
	private int[] inventory;
	private int itemsHolding;
	private int itemsMax;
	
	

	public PlayerInventoryScript() {

		//get a new inventory to use
		itemsHolding = 0;
		itemsMax = 10;
		inventory = new int[itemsMax];


	}


	public void addItem(int item) {

		if (itemsHolding < itemsMax) {

			//TODO Make more efficient
			for (int i = 0; i < itemsMax; i++) {
				if (inventory[i] == 0) {
					inventory[i] = item;
					itemsHolding++;
				}
			}
		}
		else {
			//console.log("Inventory Full");
		}

	}


	//Hopefully this should get the index of the cursor which is how we'll pick what item to remove.
	public void removeItem(int index) {
		inventory[index] = 0; //that was easy
		itemsHolding--; //even easier. 
	}

	public void swapItems(int index1, int index2) {
		int temp = inventory[index1];
		inventory[index1] = inventory[index2];
		inventory[index2] = temp;

	}




}
