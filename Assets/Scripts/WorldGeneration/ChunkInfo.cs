using UnityEngine;
using System.Collections.Generic;

public class ChunkInfo {
    public byte[,,] map { get; set; }
    public Vector3 position { get; private set; }
    public World world { get; private set; }

    public ChunkInfo(Vector3 position, World world)
    {
        map = new byte[Constants.chunkWidth, Constants.chunkHeight, Constants.chunkWidth];
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

    private byte getBlock(int x, int y, int z)
    {
        Vector3 pos = new Vector3(x, y, z) + position;
        return world.getPotentialBlock(pos);
    }
}
