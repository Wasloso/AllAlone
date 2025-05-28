using UnityEngine;

[CreateAssetMenu(menuName = "World/Generation Settings")]
public class WorldGenerationSettings : ScriptableObject
{
    [Header("Voronoi Biome Parameters")] [Tooltip("Number of Voronoi cells (distinct regions) in the world.")]
    public int numVoronoiCells = 20;

    [Tooltip("Strength of blending Voronoi noise with Perlin noise (0 = pure Voronoi, 1 = pure Perlin blend).")]
    [Range(0f, 1f)]
    public float voronoiPerlinBlendStrength = 0.2f;

    [Header("World Dimensions")] public int worldWidth = 100;

    public int worldHeight = 100;

    [Header("Noise Parameters")] [Tooltip("Seed for random number generation. Same seed = same world.")]
    public int seed;

    public float noiseScale = 20f;

    [Tooltip("Number of layers of noise for detail (Fractal Brownian Motion).")]
    public int octaves = 4;

    [Tooltip("How much influence each successive octave has (0-1).")] [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Tooltip("How much the frequency increases for each octave (>1).")]
    public float lacunarity = 2f;

    [Header("Biome Data")] [Tooltip("List of all possible biomes in your world.")]
    public BiomeData[] allBiomes;

    [Tooltip("Default biome for areas that don't match any specific biome criteria (e.g., Water).")]
    public BiomeData defaultBiome;

    [Header("Visuals")] [Tooltip("Size of one tile in Unity world units.")]
    public float tileSize = 1f;


    public Vector2 noiseOffset = Vector2.zero;
}