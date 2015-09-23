using UnityEngine;

//Author: Robert Young

public class Noise
{
    private const byte dimension = 16;
    
    private static ushort maxValue = dimension * dimension * dimension;
    
    private static ushort[,,] noiseField = new ushort[dimension, dimension, dimension];
    
    public static void init()
    {
        //make sure each entry has a unique value
        for (ushort x = 0; x < dimension; x++)
        {
            for (ushort y = 0; y < dimension; y++)
            {
                for (ushort z = 0; z < dimension; z++)
                {
                    noiseField[x, y, z] = (ushort)((x * dimension * dimension) + (y * dimension) + z);
                }
            }
        }

        //shuffle all of the numbers.

        //since we iterate through every single value, we know that every value will be 
        //given a completely random place in the field.
        for (ushort x = 0; x < dimension; x++)
        {
            for (ushort y = 0; y < dimension; y++)
            {
                for (ushort z = 0; z < dimension; z++)
                {
                    ushort swapX = (ushort)Mathf.Floor(Random.value * 16);
                    ushort swapY = (ushort)Mathf.Floor(Random.value * 16);
                    ushort swapZ = (ushort)Mathf.Floor(Random.value * 16);
                    swap(x, y, z, swapX, swapY, swapZ);
                }
            }
        }
    }

    public static float getNoiseValue(float x, float y, float z)
    {
        //wrap the x-value to be between 0 and dimension
        x %= dimension;
        x = (x < 0) ? (x + dimension) : x;
        //wrap the y-value to be between 0 and dimension
        y %= dimension;
        y = (y < 0) ? (y + dimension) : y;
        //wrap the z-value to be between 0 and dimension
        z %= dimension;
        z = (z < 0) ? (z + dimension) : z;

        //get the eight corners around the point (x,y,z), wrapping as necessary
        //	note that we will pretend that maxX is just 1 greater than minX, 
        //	but to find the values in the noise field, it needs to wrap.
        int minX = Mathf.FloorToInt(x);
        int maxX = (minX == dimension - 1) ? 0 : (minX + 1);
        int minY = Mathf.FloorToInt(y);
        int maxY = (minY == dimension - 1) ? 0 : (minY + 1);
        int minZ = Mathf.FloorToInt(z);
        int maxZ = (minZ == dimension - 1) ? 0 : (minZ + 1);

        //perform the interpolations and return a value.
        float[,,] corner = new float[2, 2, 2];
        corner[0, 0, 0] = (float)noiseField[minX, minY, minZ] / (float)maxValue;
        corner[0, 1, 0] = (float)noiseField[minX, maxY, minZ] / (float)maxValue;
        corner[1, 0, 0] = (float)noiseField[maxX, minY, minZ] / (float)maxValue;
        corner[1, 1, 0] = (float)noiseField[maxX, maxY, minZ] / (float)maxValue;
        corner[0, 0, 1] = (float)noiseField[minX, minY, maxZ] / (float)maxValue;
        corner[0, 1, 1] = (float)noiseField[minX, maxY, maxZ] / (float)maxValue;
        corner[1, 0, 1] = (float)noiseField[maxX, minY, maxZ] / (float)maxValue;
        corner[1, 1, 1] = (float)noiseField[maxX, maxY, maxZ] / (float)maxValue;

        return interpolate3d(x, y, z, minX, minX + 1, minY, minY + 1, minZ, minZ + 1, corner);
    }

    public static float getNoiseValue(float x, float y)
    {
        //Note that I am not just calling getNoiseValue(x, y, 0f); for performance reasons.
        //		Calling that would force it to interpolate2d something it doesn't need to, 
        //		and it would take twice as long.

        //wrap the x-value to be between 0 and dimension
        x %= dimension;
        x = (x < 0) ? (x + dimension) : x;
        //wrap the y-value to be between 0 and dimension
        y %= dimension;
        y = (y < 0) ? (y + dimension) : y;

        //get the eight corners around the point (x,y,z), wrapping as necessary
        //	note that we will pretend that maxX is just 1 greater than minX, 
        //	but to find the values in the noise field, it needs to wrap.
        int minX = Mathf.FloorToInt(x);
        int maxX = (minX == dimension - 1) ? 0 : (minX + 1);
        int minY = Mathf.FloorToInt(y);
        int maxY = (minY == dimension - 1) ? 0 : (minY + 1);
        //perform the interpolations and return a value.
        float[,] corner = new float[2, 2];
        corner[0, 0] = (float)noiseField[minX, minY, 0] / (float)maxValue;
        corner[0, 1] = (float)noiseField[minX, maxY, 0] / (float)maxValue;
        corner[1, 0] = (float)noiseField[maxX, minY, 0] / (float)maxValue;
        corner[1, 1] = (float)noiseField[maxX, maxY, 0] / (float)maxValue;

        return interpolate2d(x, y, minX, minX + 1, minY, minY + 1, corner);
    }

    private static float interpolate3d(float x, float y, float z, float minX, float maxX, float minY, float maxY, float minZ, float maxZ, float[,,] corners)
    {
        float[,] topCorners = new float[2, 2];
        topCorners[0, 0] = corners[0, 0, 1];
        topCorners[1, 0] = corners[1, 0, 1];
        topCorners[0, 1] = corners[0, 1, 1];
        topCorners[1, 1] = corners[1, 1, 1];
        float top = interpolate2d(x, y, minX, maxX, minY, maxY, topCorners);

        float[,] bottomCorners = new float[2, 2];
        bottomCorners[0, 0] = corners[0, 0, 0];
        bottomCorners[1, 0] = corners[1, 0, 0];
        bottomCorners[0, 1] = corners[0, 1, 0];
        bottomCorners[1, 1] = corners[1, 1, 0];
        float bottom = interpolate2d(x, y, minX, maxX, minY, maxY, bottomCorners);

        return interpolate1d(z, minZ, maxZ, bottom, top);
    }
    
    private static float interpolate2d(float x, float y, float minX, float maxX, float minY, float maxY, float[,] corner)
    {
        //TODO make the corners a float[2,2] instead of a bunch of individual floats.
        //interpolate along the bottom (corner 00 to 01)
        float bottom = interpolate1d(x, minX, maxX, corner[0, 0], corner[1, 0]);

        //interpolate along the top (corner 10 to 11)
        float top = interpolate1d(x, minX, maxX, corner[0, 1], corner[1, 1]);

        //interpolate between the two values we got from the previous interpolations.
        return interpolate1d(y, minY, maxY, bottom, top);
    }

    private static float interpolate1d(float x, float x0, float x1, float y0, float y1)
    {
        //we don't have to worry about x1 and x0 being the same value (dividing by 0),
        //since all values in the noiseField are unique.
        return y0 + ((y1 - y0) * (x - x0) / (x1 - x0));
    }

    private static void swap(ushort x1, ushort y1, ushort z1, ushort x2, ushort y2, ushort z2)
    {
        noiseField[x1, y1, z1] ^= noiseField[x2, y2, z2];
        noiseField[x2, y2, z2] ^= noiseField[x1, y1, z1];
        noiseField[x1, y1, z1] ^= noiseField[x2, y2, z2];
    }
}