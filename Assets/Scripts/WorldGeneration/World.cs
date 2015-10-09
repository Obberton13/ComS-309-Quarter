using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    [SerializeField]
    private GameObject _prefab;
    private Dictionary<int, Dictionary<int, Chunk>> _chunks = new Dictionary<int, Dictionary<int, Chunk>>();
    private Queue<ChunkInfo> _needsGenerated;
    private Queue<ChunkInfo> _needsMesh;
    private volatile bool _running;
    private System.Threading.Thread generationThread1;
    
    void Awake()
    {
        Random.seed = 1;
        Noise.init();
        _needsGenerated = new Queue<ChunkInfo>();
        _needsMesh = new Queue<ChunkInfo>();
        _running = true;
        generationThread1 = new System.Threading.Thread(generateChunks);
        generationThread1.Start();
    }

    void Start()
    {
        for (int x = -8; x < 9; x++)
        {
            for (int z = -8; z < 9; z++)
            {
                ChunkInfo info = new ChunkInfo(new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), this);
                lock(_needsGenerated)
                {
                    _needsGenerated.Enqueue(info);
                }
            }
        }
    }

    void Update()
    {
        ChunkInfo info = null;
        lock (_needsMesh)
        {
            if (_needsMesh.Count == 0) return;
            info = _needsMesh.Dequeue();
        }
        GameObject obj = (GameObject)GameObject.Instantiate(_prefab, info.position, Quaternion.identity);
        Chunk chunk = ((Chunk)obj.GetComponent<Chunk>());
        chunk.setInfo(info);
        chunk.generateMesh();
    }

    void OnApplicationQuit()
    {
        _running = false;
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

    private void generateChunks()
    {
        while (_running)
        {
            ChunkInfo toGenerate;
            lock (_needsGenerated)
            {
                if (_needsGenerated.Count == 0) continue;
                toGenerate = _needsGenerated.Dequeue();
            }
            toGenerate.generate();
            lock(_needsMesh)
            {
                _needsMesh.Enqueue(toGenerate);
            }
        }
    }
}
