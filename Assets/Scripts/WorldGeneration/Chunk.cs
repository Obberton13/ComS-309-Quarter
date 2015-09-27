using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

    public const int chunkSize = 20;
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
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    int newX = x + chunkSize * chunkX;
                    int newY = y;
                    int newZ = z + chunkSize * chunkZ;
                    float noise = Noise.getNoiseValue(newX / 8f, newY / 8f, newZ / 8f);
                    noise *= ((float)newY) / chunkSize;
                    if (noise < .05f)
                    {
                        GameObject.Instantiate(_block, new Vector3(newX, newY, newZ), Quaternion.identity);
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
