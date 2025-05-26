using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float biomeScale;

    public BiomeData[] biomes;

    private Tile[,] tiles;

    private void Awake()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        tiles = new Tile[width, height];
        var tileSize = 2f; // because your tile scale is 2x2

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            var noise = Mathf.PerlinNoise(x * biomeScale, y * biomeScale);
            var biome = ChooseBiome(noise);

            var pos = new Vector3(x * tileSize, 0, y * tileSize);

            var groundTilePrefab = biome.GetRandomGroundTile();
            if (groundTilePrefab == null) continue;

            var rotation = Quaternion.Euler(90f, 0f, 0f);
            var groundTile = Instantiate(biome.GetRandomGroundTile(), pos, rotation, transform);
            var objPrefab = biome.GetRandomObject();
            if (objPrefab != null)
                Instantiate(objPrefab, pos, Quaternion.identity, transform);

            tiles[x, y] = new Tile { position = new Vector2Int(x, y), biomeData = biome, tileObject = groundTile };
        }
    }


    private BiomeData ChooseBiome(float noise)
    {
        // Simple example: choose biome based on noise thresholds
        return biomes[Random.Range(0, biomes.Length)];
    }
}

public class Tile
{
    public BiomeData biomeData;
    public GameObject objectOnTile;
    public Vector2Int position;
    public GameObject tileObject;
}