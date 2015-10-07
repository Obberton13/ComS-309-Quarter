using UnityEngine;
using System.Collections.Generic;

public class ChunkInfo {
    public Mesh mesh { get; private set; }

    public byte[,,] map { get; private set; }
    public Vector3 position { get; private set; }
    public World world { get; private set; }

    public ChunkInfo(Vector3 position, World world)
    {
        map = new byte[Constants.chunkWidth, Constants.chunkHeight, Constants.chunkWidth];
        mesh = new Mesh();
        this.world = world;
        this.position = position;
    }

    public void generate()
    {
        for (int x = 0; x < Constants.chunkWidth; x++)
        {
            for (int z = 0; z < Constants.chunkWidth; z++)
            {
                for (int y = 0; y < Constants.chunkHeight; y++)
                {
                    map[x, y, z] = getBlock(x, y, z);
                }
            }
        }
    }

    public void generateMesh()
    {
        mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < Constants.chunkWidth; x++)
        {
            for (int z = 0; z < Constants.chunkWidth; z++)
            {
                for (int y = 0; y < Constants.chunkHeight; y++)
                {
                    if (map[x, y, z] == 0)
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
    }
    
    private void buildBlock(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        //Front face
        if (isTransparent(x, y, z - 1))
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.up, origin + Vector3.right, origin + Vector3.up + Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
        //back face
        if (isTransparent(x, y, z + 1))
        {
            Vector3 origin = new Vector3(x + 1, y, z + 1);
            Vector3[] corners = { origin, origin + Vector3.up, origin - Vector3.right, origin + Vector3.up - Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
        if (isTransparent(x + 1, y, z))
        //right face
        {
            Vector3 origin = new Vector3(x + 1, y, z);
            Vector3[] corners = { origin, origin + Vector3.up, origin + Vector3.forward, origin + Vector3.up + Vector3.forward };
            buildFace(corners, verts, uvs, tris);
        }
        //left face
        if (isTransparent(x - 1, y, z))
        {
            Vector3 origin = new Vector3(x, y, z);
            Vector3[] corners = { origin, origin + Vector3.forward, origin + Vector3.up, origin + Vector3.forward + Vector3.up };
            buildFace(corners, verts, uvs, tris);
        }
        //top face
        if (isTransparent(x, y + 1, z))
        {
            Vector3 origin = new Vector3(x, y + 1, z);
            Vector3[] corners = { origin, origin + Vector3.forward, origin + Vector3.right, origin + Vector3.forward + Vector3.right };
            buildFace(corners, verts, uvs, tris);
        }
        //bottom face
        if (isTransparent(x, y - 1, z))
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
        Vector3 pos = new Vector3(x, y, z) + position;
        return world.getPotentialBlock(pos);
    }
}
