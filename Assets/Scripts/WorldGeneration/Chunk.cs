using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour {
    
    [SerializeField]
    private GameObject _block;

    private byte[,,] _map = new byte[Constants.chunkWidth, Constants.chunkHeight, Constants.chunkWidth];
    private MeshCollider collider;
    private MeshRenderer renderer;
    private MeshFilter filter;

    void Awake()
    {
        collider = GetComponent<MeshCollider>();
        renderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
    }

    // Use this for initialization
    void Start()
    {
        //System.Threading.Thread thread = new System.Threading.Thread(generateBlocks);
        //thread.Start();
        generateBlocks();
        generateMesh();
    }

    private void generateBlocks()
    {
        for (int x = 0; x < Constants.chunkWidth; x++)
        {
            for (int z = 0; z < Constants.chunkWidth; z++)
            {
                for (int y = 0; y < Constants.chunkHeight; y++)
                {
                    _map[x, y, z] = generateBlock(x, y, z);
                }
            }
        }
    }

    private byte generateBlock(int x, int y, int z)
    {
        float noise = Noise.getNoiseValue((new Vector3(x, y, z) + transform.position) / 16f);
        noise /= (y + 1);
        if (noise > .04f)
        {
            return 1;
        }
        return 0;
    }

    private void generateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < Constants.chunkWidth; x++)
        {
            for (int z = 0; z < Constants.chunkWidth; z++)
            {
                for(int y = 0; y < Constants.chunkHeight; y++)
                {
                    if (_map[x, y, z] == 0)
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

        filter.mesh = mesh;
        collider.sharedMesh = mesh;
    }

    private void buildBlock(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        //Front face
        { 
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.up, origin + Vector3.right, origin + Vector3.up + Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
        //back face
        {
            Vector3 origin = new Vector3(x + 1, y, z + 1);
            Vector3[] corners = { origin, origin + Vector3.up, origin - Vector3.right, origin + Vector3.up - Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
        //right face
        {
            Vector3 origin = new Vector3(x + 1, y, z);
            Vector3[] corners = { origin, origin + Vector3.up, origin + Vector3.forward, origin + Vector3.up + Vector3.forward };
            buildFace(corners, verts, uvs, tris);
        }
        //left face
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.forward, origin + Vector3.up, origin + Vector3.forward + Vector3.up };
            buildFace(corners, verts, uvs, tris);
        }
        //top face
        {
            Vector3 origin = new Vector3(x, y + 1, z);
            Vector3[] corners = { origin, origin + Vector3.forward, origin + Vector3.right, origin + Vector3.forward + Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
        //bottom face
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.right, origin + Vector3.forward, origin + Vector3.forward + Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
    }

    private void buildFace(Vector3[] corners, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corners[0]);
        verts.Add(corners[1]);
        verts.Add(corners[3]);
        verts.Add(corners[2]);

        uvs.Add(new Vector2());
        uvs.Add(Vector2.up);
        uvs.Add(Vector2.up + Vector2.right);
        uvs.Add(Vector2.right);

        tris.Add(index);
        tris.Add(index+1);
        tris.Add(index+2);
        tris.Add(index);
        tris.Add(index+2);
        tris.Add(index+3);
    }
}
