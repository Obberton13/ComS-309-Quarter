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
                GameObject.Instantiate(prefab, new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), Quaternion.identity);
            }
        }
    }
}
