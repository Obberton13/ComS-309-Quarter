using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {
    
    [SerializeField]
    private GameObject _block;
    private int chunkX, chunkZ;

    void Awake()
    {
        Random.seed = 39;
        Noise.init();
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(generateBlocks());
    }

    private IEnumerator generateBlocks()
    {
        for (int x = 0; x < Constants.chunkWidth; x++)
        {
            for (int z = 0; z < Constants.chunkWidth; z++)
            {
                for (int y = 0; y < Constants.chunkHeight; y++)
                {
                    float noise = Noise.getNoiseValue((new Vector3(x, y, z) + transform.position) / 16f);
                    noise /= (y + 1);
                    if (noise > .05f)
                    {
                        GameObject.Instantiate(_block, new Vector3(x, y, z) + transform.position, Quaternion.identity);
                    }
                }
                yield return 0;
            }
        }
    }

    public void setChunkPos(int x, int z)
    {
        this.chunkX = x;
        this.chunkZ = z;
    }

    public void setPrefab(GameObject prefab)
    {
        this._block = prefab;
    }
}
