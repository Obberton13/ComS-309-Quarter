using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    [SerializeField]
    private GameObject _prefab;
    
    void Awake()
    {
        Random.seed = 1;
        Noise.init();
    }

    void Start()
    {
        for (int x = -2; x < 3; x++)
        {
            for (int z = -2; z < 3; z++)
            {
                ChunkInfo info = new ChunkInfo(new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), this);
                info.generate();
                info.generateMesh();
                GameObject obj = (GameObject)GameObject.Instantiate(_prefab, new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), Quaternion.identity);
                ((Chunk)obj.GetComponent<Chunk>()).setInfo(info);
            }
        }
    }

    public byte getPotentialBlock(Vector3 pos)
    {
        return getPotentialBlock(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    public byte getPotentialBlock(int x, int y, int z)
    {
        if (y == 0) return 1;
        float noise1 = Noise.getNoiseValue(new Vector3(x, y, z) / 16f);
        float noise2 = Noise.getNoiseValue(new Vector3(x, y, z) / 37f);
        float noise4 = Noise.getNoiseValue(new Vector3(x, y, z) / 4f);
        noise1 += noise2*25 + noise4;
        noise1 /= 27;
        noise1 /= (y + 1);
        if (noise1 > .04f && noise2 < .7)
        {
            return 1;
        }
        return 0;
    }
}
