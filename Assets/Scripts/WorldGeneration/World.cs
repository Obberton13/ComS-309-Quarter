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
                //TODO instead of instantiate, generate all the vertices and tris.
                //yield return 0;
            }
        }
    }

    void Update()
    {
        createChunks();
    }

    private IEnumerator createChunks()
    {

        while (true)
        {
            yield return 0;
        }
    }
}
