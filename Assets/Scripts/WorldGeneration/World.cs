using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
    public GameObject prefab;

    void Start()
    {
        for(int x = -2; x < 3; x++)
        {
            for(int z = -2; z < 3; z++)
            {
                Chunk chunk = GameObject.Instantiate(prefab).GetComponent<Chunk>();
                chunk.setChunkPos(x, z);
            }
        }
    }
}
