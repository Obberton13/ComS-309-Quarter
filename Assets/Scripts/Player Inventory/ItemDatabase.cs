using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    void Awake()
    {
        // StreamableAssets makes it so that we don't have to recompile the game every time we change the database
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/blocks.json"));
        ConstructBlockDatabase();
    }

    void ConstructBlockDatabase()
    {
        for (int i = 0; i < itemData.Count; ++i)
        {
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["color"].ToString()));
        }
    }

    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < database.Count; ++i)
        {
            if (database[i].ID == id)
                return database[i];
        }
        return null;
    }
}

public class Item
{
    public int ID { get; set; }
    public string Color { get; set; }
    public Sprite sprite { get; set; }

    public Item()
    {
        ID = -1;
        //Color = "air";
    }

    public Item(int id, string color)
    {
        this.ID = id;
        this.Color = color;
        Debug.Log(this.Color + "_block");
        //this.sprite = Resources.Load<Sprite>("Sprites/Items/" + this.Color + "_block");
        if (id > 0)
            this.sprite = Resources.Load<Sprite>("Sprites/Items/inv_" + this.Color);
        //else this.sprite = null;
        Debug.Log(this.sprite);
    }
}
