using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    [SerializeField]
    private GameObject _prefab;

    private Queue _needsGenerated = new Queue();
    private Queue _doneGenerating = new Queue();

    
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
                GameObject.Instantiate(_prefab, new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), Quaternion.identity);
            }
        }
    }

    public byte getPotentialBlock(Vector3 pos)
    {
        return getPotentialBlock(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    public byte getPotentialBlock(int x, int y, int z)
    {
        float noise = Noise.getNoiseValue(new Vector3(x, y, z) / 16f);
        noise /= (y + 1);
        if (noise > .04f)
        {
            return 1;
        }
        return 0;
    }
}
