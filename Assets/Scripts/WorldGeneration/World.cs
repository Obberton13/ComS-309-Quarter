using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    [SerializeField]
    private GameObject _block;

    void Awake()
    {
        Random.seed = 1;
        Noise.init();
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(generateBlocks());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator generateBlocks()
    {
        for(int x = -10; x < 10; x++)
        {
            for(int y = -10; y < 10; y++)
            {
                for(int z = -10; z < 10; z++)
                {
                    float noise = Noise.getNoiseValue(x / 6f, y / 6f, z / 6f);
                    if (noise < .4f)
                    {
                        GameObject.Instantiate(_block, new Vector3(x, y, z), Quaternion.identity);
                        yield return 0;
                    }
                }
            }
        }
    }
}
