using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class ChunkInfo {
    public byte[,,] map { get; set; }
	public float x;
	public float y;
	public float z;
	//private World world;

    public ChunkInfo(Vector3 position, World world)
    {
        map = new byte[Constants.chunkWidth, Constants.chunkHeight, Constants.chunkWidth];
        //this.world = world;
		x = position.x;
		y = position.y;
		z = position.z;
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
        Vector3 pos = new Vector3(x, y, z) + new Vector3(this.x, this.y, this.z);
        return World.getPotentialBlock(pos);
    }

	public Vector3 getPos()
	{
		return new Vector3(this.x, this.y, this.z);
	}
}
