using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	int chunk_count = 0;

    [SerializeField]
    private GameObject _prefab;
    private Dictionary<int, Dictionary<int, Chunk>> _chunks = new Dictionary<int, Dictionary<int, Chunk>>();
	private List<ChunkInfo> _infos = new List<ChunkInfo> ();
	private Queue<ChunkInfo> _needsGenerated;
    private Queue<ChunkInfo> _needsMesh;
    private Queue<ChunkInfo> _fullyGenerated;
    private volatile bool _running;
	public volatile bool _isGenerating;
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
		_isGenerating = true;
    }

    void Start()
    {
		GameObject.Find ("Game Controller").GetComponent<MenuState>().menuState = MenuState.MenuStates.inMainMenu;
        for (int x = -10; x < 10; x++)
        {
            for (int z = -10; z < 10; z++)
            {
                ChunkInfo info = new ChunkInfo(new Vector3(Constants.chunkWidth * x, 0, Constants.chunkWidth * z), this);
                lock(_needsGenerated)
                {
                    _needsGenerated.Enqueue(info);
					_infos.Add (info);
                }
			}
        }
    }

    void Update()
    {
        ChunkInfo info = null;
        lock (_needsMesh)
        {
            if (_needsMesh.Count == 0){
				_isGenerating = false;
				return;
			}
            info = _needsMesh.Dequeue();
        }
        GameObject obj = (GameObject) Instantiate(_prefab, info.getPos(), Quaternion.identity);
		//GameObject obj = (GameObject)PhotonNetwork.Instantiate("Chunk", info.getPos(), Quaternion.identity, 0);
        Chunk chunk = ((Chunk)obj.GetComponent<Chunk>());
        chunk.setInfo(info);
        chunk.generateMesh();
        Vector3 pos = info.getPos();
        int x = Mathf.FloorToInt(pos.x / Constants.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / Constants.chunkWidth);
        if(!_chunks.ContainsKey(x))
        {
            _chunks.Add(x, new Dictionary<int, Chunk>());
        }
        _chunks[x].Add(z, chunk);
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
                if (_needsGenerated.Count == 0) 
				{
					_isGenerating = false;
					continue;
				}
				_isGenerating = true;
                toGenerate = _needsGenerated.Dequeue();
            }
            toGenerate.generate();
            lock(_needsMesh)
            {
                _needsMesh.Enqueue(toGenerate);
            }
        }
    }
	
	/* Returns (Array)List of all generated ChunkInfo objects */
	public List<ChunkInfo> getChunkInfos()
	{
		return _infos;
	}

	/* Sets the current ChunkInfos to the (Array)List given */
	public void setChunkInfos(List<ChunkInfo> input)
	{
		_infos.Clear();
		_infos = input;

		// Iterate through provided ChunkInfos, queue for mesh_generation
		foreach ( ChunkInfo info in input )
		lock(_needsGenerated)
		{
			_isGenerating = true;
			_needsMesh.Enqueue(info);
		}
	}

    public void putBlock(int x, int y, int z, byte type)
    {
        int chunkX = Mathf.FloorToInt((float)x / Constants.chunkWidth);
        int chunkZ = Mathf.FloorToInt((float)z / Constants.chunkWidth);
        Chunk chunk = _chunks[chunkX][chunkZ];
        //How many debug.logs does it take to realize that integer division is a thing that truncates toward 0?
        //Debug.Log(x / Constants.chunkWidth);
        //Debug.Log(Mathf.FloorToInt(-9.5f));
        //Debug.Log(Mathf.FloorToInt(x / Constants.chunkWidth));
        //Debug.Log("Set Block: " + x + ", " + y + ", " + z + " to Type: " + type);
        //Debug.Log("On chunk: " + chunkX + ", " + chunkZ);
        chunk.getInfo().map[(int)Mathf.Repeat(x, Constants.chunkWidth), y, (int)Mathf.Repeat(z, Constants.chunkWidth)] = type;
        chunk.generateMesh();

    }
}
