using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour {
    private MeshCollider _collider;
    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private ChunkInfo _info;

    void Awake()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();
        _filter = GetComponent<MeshFilter>();
    }

    // Use this for initialization
    void Start()
    {

    }

    public void setInfo(ChunkInfo info)
    {
        _info = info;
    }

	public ChunkInfo getInfo()
	{
		return _info;
	}

    public byte getBlockAt(int x, int y, int z)
    {
        return _info.map[x, y, z];
    }

    public void generateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < Constants.chunkWidth; x++)
        {
            for (int z = 0; z < Constants.chunkWidth; z++)
            {
                for (int y = 0; y < Constants.chunkHeight; y++)
                {
                    if (_info.map[x, y, z] == 0)
                    {
                        continue;
                    }
                    buildBlock(x, y, z, verts, uvs, tris);
                }
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        _filter.mesh = mesh;
        _collider.sharedMesh = mesh;
    }

    private void buildBlock(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        byte block = _info.map[x, y, z];
        //Front face
        if (isTransparent(x, y, z - 1))
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.up, origin + Vector3.right, origin + Vector3.up + Vector3.right };
            buildFace(block, corners, verts, uvs, tris);
        }
        //back face
        if (isTransparent(x, y, z + 1))
        {
            Vector3 origin = new Vector3(x + 1, y, z + 1);
            Vector3[] corners = { origin, origin + Vector3.up, origin - Vector3.right, origin + Vector3.up - Vector3.right };
            buildFace(block, corners, verts, uvs, tris);
        }
        if (isTransparent(x + 1, y, z))
        //right face
        {
            Vector3 origin = new Vector3(x + 1, y, z);
            Vector3[] corners = { origin, origin + Vector3.up, origin + Vector3.forward, origin + Vector3.up + Vector3.forward };
            buildFace(block, corners, verts, uvs, tris);
        }
        //left face
        if (isTransparent(x - 1, y, z))
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.forward, origin + Vector3.up, origin + Vector3.forward + Vector3.up };
            buildFace(block, corners, verts, uvs, tris);
        }
        //top face
        if (isTransparent(x, y + 1, z))
        {
            Vector3 origin = new Vector3(x, y + 1, z);
            Vector3[] corners = { origin, origin + Vector3.forward, origin + Vector3.right, origin + Vector3.forward + Vector3.right };
            buildFace(block, corners, verts, uvs, tris);
        }
        //bottom face
        if (isTransparent(x, y - 1, z))
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.right, origin + Vector3.forward, origin + Vector3.forward + Vector3.right };
            buildFace(block, corners, verts, uvs, tris);
        }
    }

    private void buildFace(byte block, Vector3[] corners, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        block -= 1;

        Vector2 uvLL = new Vector2(block % 8, 7f - Mathf.Floor(block / 8)) * .125f;
        Vector2 uvUL = new Vector2(0, .125f) + uvLL;
        Vector2 uvUR = new Vector2(.125f, .125f) + uvLL;
        Vector2 uvLR = new Vector2(.125f, 0)+uvLL;

        verts.Add(corners[0]);
        verts.Add(corners[1]);
        verts.Add(corners[3]);
        verts.Add(corners[2]);

        uvs.Add(uvUL);
        uvs.Add(uvUR);
        uvs.Add(uvLR);
        uvs.Add(uvLL);

        tris.Add(index);
        tris.Add(index + 1);
        tris.Add(index + 2);
        tris.Add(index);
        tris.Add(index + 2);
        tris.Add(index + 3);
    }

    private bool isTransparent(int x, int y, int z)
    {
        if (y < 0) return true;
        if (y >= Constants.chunkHeight) return true;
        return getBlock(x, y, z) == 0;
    }

    private byte getBlock(int x, int y, int z)
    {
        if (x < Constants.chunkWidth && x >= 0 && z < Constants.chunkWidth && z >= 0 && y < Constants.chunkHeight && y >= 0) return _info.map[x, y, z];
        return 0;
    }
}
