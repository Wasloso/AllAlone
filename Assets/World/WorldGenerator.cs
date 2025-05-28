using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int worldWidth = 100;
    public int worldHeight = 100;
    public int biomeCount = 25;

    public BiomeData[] biomeTypes;
    public float biomeInfluenceRadius = 30f; // How far a biome "reaches" with influence
    private BiomeData[] biomeAssignments;

    private Vector2[] biomeCenters;
    private Tile[,] tiles;

    private void Awake()
    {
        GenerateBiomeCenters();
        AssignTilesToBiomes();
        InstantiateTiles();
    }

    private void GenerateBiomeCenters()
    {
        biomeCenters = new Vector2[biomeCount];
        biomeAssignments = new BiomeData[biomeCount];

        for (var i = 0; i < biomeCount; i++)
        {
            // Spread biome centers around (0,0)
            var x = Random.Range(-worldWidth / 2f, worldWidth / 2f);
            var y = Random.Range(-worldHeight / 2f, worldHeight / 2f);
            biomeCenters[i] = new Vector2(x, y);

            biomeAssignments[i] = biomeTypes[Random.Range(0, biomeTypes.Length)];
        }
    }

    private void AssignTilesToBiomes()
    {
        tiles = new Tile[worldWidth, worldHeight];

        // Coordinates offset so (0,0) is in the center of the grid
        var halfWidth = worldWidth / 2f;
        var halfHeight = worldHeight / 2f;

        for (var x = 0; x < worldWidth; x++)
        for (var y = 0; y < worldHeight; y++)
        {
            // Translate grid coords to world coords centered on (0,0)
            var tilePos = new Vector2(x - halfWidth, y - halfHeight);

            // Calculate biome influence weights
            var maxInfluence = float.MinValue;
            var chosenBiomeIndex = 0;

            for (var i = 0; i < biomeCenters.Length; i++)
            {
                var dist = Vector2.Distance(tilePos, biomeCenters[i]);

                // Gaussian-style falloff, smooth blob shape
                var influence = Mathf.Exp(-(dist * dist) / (2 * biomeInfluenceRadius * biomeInfluenceRadius));

                if (influence > maxInfluence)
                {
                    maxInfluence = influence;
                    chosenBiomeIndex = i;
                }
            }

            tiles[x, y] = new Tile
            {
                position = new Vector2Int(x, y),
                biomeData = biomeAssignments[chosenBiomeIndex]
            };
        }
    }

    private void InstantiateTiles()
    {
        var halfWidth = worldWidth / 2f;
        var halfHeight = worldHeight / 2f;

        for (var x = 0; x < worldWidth; x++)
        for (var y = 0; y < worldHeight; y++)
        {
            var pos = new Vector3(x - halfWidth, 0, y - halfHeight);
            var biome = tiles[x, y].biomeData;

            var tile = biome.GetRandomGroundTile();

            Instantiate(tile, pos, tile.transform.rotation, transform);
            // Optional: instantiate biome objects too
        }
    }
}

public class Tile
{
    public BiomeData biomeData;
    public Vector2Int position;
}