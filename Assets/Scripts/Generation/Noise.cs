using UnityEngine;

//Author: Robert Young

/// <summary>
///		A basic implementation of Perlin Noise using Linear Interpolation instead of bicubic or whatever.
///		Linear interpolation is a lot easier to do.
/// </summary>
public class Noise {
	
	/// <summary>
	///		this is the size of the three dimensional noise field in each direction (making a cube)
	/// </summary>
	private const byte dimension = 16;

	/// <summary>
	///		This is the maximum possible value that any number can have in the noise field.
	///		Used when getting float values between 0.0f and 1.0f (we can just divide by this)
	/// </summary>
	private static ushort maxValue = dimension * dimension * dimension;

	/// <summary>
	///		This is the noise field itself. When we call our noise function, these are the
	///		values that we will interpolate between in order to come up with terrain that looks halfway decent.
	/// </summary>
	private static ushort[,,] noiseField = new ushort[dimension,dimension,dimension];
	
	/// <summary>
	///		Initializes the noisefield and puts all of the numbers in random locations
	///		Call this function after setting your seed, if you want one set,
	///		and before you do anything with noise.
	/// 
	///		Application-specific, this function should be called before generating any world,
	///		including on game startup.
	/// </summary>
	public static void init()
	{
		//make sure each entry has a unique value
		for(ushort x = 0; x < dimension; x++)
		{
			for(ushort y = 0; y < dimension; y++)
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

	/// <summary>
	///		Gets a float value between 0f and 1f based on the x, y, and z values given.
	/// 
	///		Small changes in the x, y, and z values will result in very small changes in 
	///		the return value, while larger changes will result in larger variance.
	///		
	///		Passing the same values for x, y, and z will get you the same value every time.
	/// 
	///		This version is three dimensional, so you can look things up based on 
	///		an x, y, and z value.
	/// 
	///		It doesn't matter which axis is which when specifying x, y and z values. The 
	///		field we look up these values are is a cube. 
	///		Just be sure to be consistent when you do it.
	/// </summary>
	/// <param name="x">float value in the X direction</param>
	/// <param name="y">float value in the Y direction</param>
	/// <param name="z">float value in the Z direction</param>
	/// <returns>float value between 0f and 1f based on the x, y, and z given</returns>
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

		return interpolate3d(x, y, z, minX, maxX, minY, maxY, minZ, maxZ, corner);
	}

	/// <summary>
	///		Gets a float value between 0f and 1f based on the x, y, and z values given.
	/// 
	///		Small changes in the x, y, and z values will result in very small changes in 
	///		the return value, while larger changes will result in larger variance.
	///		
	///		Passing the same values for x, y, and z will get you the same value every time.
	/// 
	///		This version is two dimensional, so you can only look things up based on 
	///		an x and y value.
	/// </summary>
	/// <param name="x">float value in the X direction</param>
	/// <param name="y">float value in the Y direction</param>
	/// <returns>float value between 0f and 1f based on the x, and y given</returns>
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

		return interpolate2d(x, y, minX, maxX, minY, maxY, corner);
	}

	/// <summary>
	///		Trilinear interpolation.
	/// 
	///		You can think of it like this:
	///		You are working in a 4-dimensional space, and you know the w-value of the points at
	///		(minX, minY), (minX, maxY), (maxX, minY), and (maxX, maxY), for minZ, as well as maxZ
	/// 
	///		This finds the approximation of the w-value with the given x, y, and z based on trilinear interpolation.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <param name="minX"></param>
	/// <param name="maxX"></param>
	/// <param name="minY"></param>
	/// <param name="maxY"></param>
	/// <param name="minZ"></param>
	/// <param name="maxZ"></param>
	/// <param name="corners"></param>
	/// <returns></returns>
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

	/// <summary>
	///		Bilinear interpolation.
	/// 
	///		You can think of it like this:
	///		You are working in a 3-dimensional space, and you know the z-value of the points at
	///		(minX, minY), (minX, maxY), (maxX, minY), and (maxX, maxY).
	/// 
	///		This finds the approximation of the z-value with the given x and y, based on bilinear interpolation.
	/// </summary>
	/// <param name="x">Value between minX and maxX to interpolate in the x-direction</param>
	/// <param name="y">Value between minX and maxX to interpolate in the y-direction</param>
	/// <param name="minX">Minimum x of the plane you are interpolating on</param>
	/// <param name="maxX">Maximum x of the plane you are interpolating on</param>
	/// <param name="minY">Minimum y of the plane you are interploating on</param>
	/// <param name="maxY">Maximum y of the plane you are interpolating on</param>
	/// <param name="corner">
	///		The 2x2 array of the corner values that you are interpolating between.
	///		0,0 should be the value of minX, minY
	///		1,0 should be the value of maxX, minY
	///		0,1 should be the value of minX, maxY
	///		1,1 should be the value of maxX, maxY
	///	</param>
	/// <returns>The interpolated value at (x,y)</returns>
	private static float interpolate2d(float x, float y, float minX, float maxX,  float minY, float maxY, float[,] corner)
	{
		//TODO make the corners a float[2,2] instead of a bunch of individual floats.
		//interpolate along the bottom (corner 00 to 01)
		float bottom = interpolate1d(x, minX, maxX, corner[0,0], corner[1,0]);

		//interpolate along the top (corner 10 to 11)
		float top = interpolate1d(x, minX, maxX, corner[0,1], corner[1,1]);

		//interpolate between the two values we got from the previous interpolations.
		return interpolate1d(y, minY, maxY, bottom, top);
	}

	/// <summary>
	///		Linear interpolation.
	/// 
	///		x1 is more positive in the noiseField than x0, but not necessarily having 
	///		greater value, and not necessarily in the x-direction.
	/// 
	///		Same goes with y1, being more positive in the y-direction.
	/// 
	///		For instance, your values that you might pass into this function might be
	///		x = 2.68902f
	///		x0 = 2f;
	///		x1 = 3f;
	///		y0 = ((float)noiseField[2, 5, 5])/maxValue;
	///		y1 = ((float)noiseField[3, 5, 5])/maxValue;
	/// 
	///		x will usually be between x0 and x1, but does not have to be.
	/// </summary>
	/// <param name="x">How far from x0 to x1 you want to find y if y is from y0 to y1</param>
	/// <param name="x0">The first end of the x-axis</param>
	/// <param name="x1">The other end of the x-axis</param>
	/// <param name="y0">The first end of the y-axis</param>
	/// <param name="y1">The other end of the y-axis</param>
	/// <returns></returns>
	private static float interpolate1d(float x, float x0, float x1, float y0, float y1)
	{
		//we don't have to worry about x1 and x0 being the same value (dividing by 0),
		//since all values in the noiseField are unique.
		return y0 + ((y1 - y0) * (x - x0) / (x1 - x0));
	}

	/// <summary>
	///		Swaps 2 values in the noiseField.
	/// </summary>
	/// <param name="x1">x of the first value to be swapped</param>
	/// <param name="y1">y of the first value to be swapped</param>
	/// <param name="z1">z of the first value to be swapped</param>
	/// <param name="x2">x of the second value to be swapped</param>
	/// <param name="y2">y of the second value to be swapped</param>
	/// <param name="z2">z of the second value to be swapped</param>
	private static void swap(ushort x1, ushort y1, ushort z1, ushort x2, ushort y2, ushort z2)
	{
		noiseField[x1, y1, z1] ^= noiseField[x2, y2, z2];
		noiseField[x2, y2, z2] ^= noiseField[x1, y1, z1];
		noiseField[x1, y1, z1] ^= noiseField[x2, y2, z2];
    }
}
