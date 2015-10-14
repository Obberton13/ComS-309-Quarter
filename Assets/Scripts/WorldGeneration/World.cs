using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

    [SerializeField]
    private GameObject _prefab;
    private Dictionary<int, Dictionary<int, Chunk>> _chunks = new Dictionary<int, Dictionary<int, Chunk>>();
	private List<ChunkInfo> _infos = new List<ChunkInfo> ();
	private Queue<ChunkInfo> _needsGenerated;
    private Queue<ChunkInfo> _needsMesh;
    private Queue<ChunkInfo> _fullyGenerated;
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
        //generationThread1.Start();
    }

    void Start()
    {
        for (int x = -10; x < 10; x++)
        {
            for (int z = -10; z < 10; z++)
            {
                ChunkInfo info = new ChunkInfo(new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), this);
                lock(_needsGenerated)
                {
                    _needsGenerated.Enqueue(info);
					_infos.Add(info);
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
        GameObject obj = (GameObject)GameObject.Instantiate(_prefab, info.getPos(), Quaternion.identity);
        Chunk chunk = ((Chunk)obj.GetComponent<Chunk>());
        chunk.setInfo(info);
        chunk.generateMesh();
    }

    void OnApplicationQuit()
    {
        _running = false;
    }

    public static byte getPotentialBlock(Vector3 pos)
    {
        return getPotentialBlock(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    public static byte getPotentialBlock(int x, int y, int z)
    {
        if (y == 0) return 1;
        float noise1 = Noise.getNoiseValue(new Vector3(x, y, z) / 16f);
        float noise2 = Noise.getNoiseValue(new Vector3(x, y, z) / 37f);
        float noise4 = Noise.getNoiseValue(new Vector3(x, y, z) / 4f);
        noise1 += noise2*25 + noise4;
        noise1 /= 27;
        noise1 /= (y + 1);
        if (noise1 > .04f)
        {
            return 1;
        }
        if (noise2 < .2) return 2;
        return 0;
    }

    public byte getActualBlock(Vector3 pos)
    {
        return getActualBlock(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    public byte getActualBlock(int x, int y, int z)
    {
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

	public List<ChunkInfo> getChunkInfos()
	{
		return _infos;
	}

	public void setChunkInfos(List<ChunkInfo> input)
	{
		_infos = input;
		foreach ( ChunkInfo info in input )
		lock(_needsGenerated)
		{
			_needsMesh.Enqueue(info);
		}
	}
}
