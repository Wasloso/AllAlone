using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, int octaves, float persistence,
        float lacunarity, Vector2 offset, int seed)
    {
        var noiseMap = new float[width, height];


        var prng = new Random(seed);
        var octaveOffsets = new Vector2[octaves];
        for (var i = 0; i < octaves; i++)
        {
            var offsetX = prng.Next(-100000, 100000) + offset.x;
            var offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.0001f;

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;


        var halfWidth = width / 2f;
        var halfHeight = height / 2f;

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for (var i = 0; i < octaves; i++)
            {
                var sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                var sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseHeight += perlinValue * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
            if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
            noiseMap[x, y] = noiseHeight;
        }


        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);

        return noiseMap;
    }


    public static float[,] GenerateVoronoiMap(int width, int height, int numCells, int seed, float blendStrength = 0.5f)
    {
        var voronoiMap = new float[width, height];


        var prng = new Random(seed);


        var cellPoints = new List<Vector2>();
        for (var i = 0; i < numCells; i++) cellPoints.Add(new Vector2(prng.Next(0, width), prng.Next(0, height)));


        float maxDist = 0;
        var minDist = float.MaxValue;


        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var closestDist = float.MaxValue;
            var secondClosestDist = float.MaxValue;

            foreach (var cellPoint in cellPoints)
            {
                var dist = Vector2.Distance(new Vector2(x, y), cellPoint);
                if (dist < closestDist)
                {
                    secondClosestDist = closestDist;
                    closestDist = dist;
                }
                else if (dist < secondClosestDist)
                {
                    secondClosestDist = dist;
                }
            }


            var value = secondClosestDist - closestDist;


            voronoiMap[x, y] = value;


            if (value > maxDist) maxDist = value;
            if (value < minDist) minDist = value;
        }


        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            voronoiMap[x, y] = Mathf.InverseLerp(minDist, maxDist, voronoiMap[x, y]);


            var perlinBlend = Mathf.PerlinNoise(
                (x + seed) / (width * 0.1f),
                (y + seed) / (height * 0.1f)
            );
            voronoiMap[x, y] = Mathf.Lerp(voronoiMap[x, y], perlinBlend, blendStrength);
        }

        return voronoiMap;
    }

    public static int[,] GenerateVoronoiRegionMap(int width, int height, int numCells, int seed,
        out Vector2[] cellPoints)
    {
        var regionMap = new int[width, height];
        var prng = new Random(seed);

        cellPoints = new Vector2[numCells];
        for (var i = 0; i < numCells; i++)
            cellPoints[i] = new Vector2(prng.Next(0, width), prng.Next(0, height));

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var closestIndex = 0;
            var closestDist = float.MaxValue;

            for (var i = 0; i < cellPoints.Length; i++)
            {
                var dist = Vector2.SqrMagnitude(cellPoints[i] - new Vector2(x, y));
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i;
                }
            }

            regionMap[x, y] = closestIndex;
        }

        return regionMap;
    }
}