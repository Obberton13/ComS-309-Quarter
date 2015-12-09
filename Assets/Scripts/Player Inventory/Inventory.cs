using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    ItemDatabase database;
    GameObject inventoryPanel;
    GameObject slotPanel;
    // The slot prefab (assigned in the editor)
    public GameObject inventorySlot;
    // The item prefab (assigned in the editor)
    public GameObject inventoryItem;

    public int numSlots;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        database = GetComponent<ItemDatabase>();

        //numSlots = 30;
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject;

        for(int i = 0; i < numSlots; ++i)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }
        AddItem(0);
        AddItem(1);
        AddItem(0);
        AddItem(1);
        AddItem(2);
        AddItem(3);
    }

    public void AddItem(int id)
    {
        Item adding = database.FetchItemByID(id);

        int check = checkInInventory(adding);
        if (check > -1)
        {
            ItemData data = slots[check].transform.GetChild(0).GetComponent<ItemData>();
            data.amount++;
            data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
        }
        else
        {
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].ID == -1)
                {
                    items[i] = adding;
                    GameObject obj = Instantiate(inventoryItem);
                    obj.transform.SetParent(slots[i].transform);
                    obj.GetComponent<Image>().sprite = adding.sprite;
                    obj.transform.position = Vector2.zero;
                    //obj.name = adding.Color + " Block";
                    slots[i].name = adding.Color + " Block";
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    break;
                }
            }
        }
    }

    public bool RemoveItem(int slot)
    {
        ItemData data = slots[slot].transform.GetChild(0).GetComponent<ItemData>();
        if(data.amount > 0)
        {
            data.amount--;
            data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
            if (data.amount == 0)
            {
                items[slot] = new Item();
            }
            return true;
        }
        return false;
    }

    // Returns slot that item current exists in
    int checkInInventory(Item item)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].ID == item.ID)
            {
                return i;
            }
        }
        return -1;
    }
}
