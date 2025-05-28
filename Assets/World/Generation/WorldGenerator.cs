using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public WorldGenerationSettings settings;


    [Tooltip("Parent object for all generated tiles for cleaner Hierarchy.")]
    public Transform worldContainer;


    private BiomeData[] _biomeAssignments;

    private BiomeData[,] _biomeMap;

    private float[,] _elevationMap;

    // These 3 are useless now
    private float[,] _humidityMap;
    private int[,] _regionIndexMap;
    private float[,] _temperatureMap;

    private void Start()
    {
        GenerateWorld();
    }

    [ContextMenu("Generate World")]
    public void GenerateWorld()
    {
        if (settings == null)
        {
            Debug.LogError("World Generation Settings are not assigned!", this);
            return;
        }

        if (worldContainer == null)
        {
            Debug.LogError("World Container is not assigned! Please create an empty GameObject and assign it.", this);
            return;
        }

        ClearExistingWorld();


        _elevationMap = NoiseGenerator.GenerateVoronoiMap(
            settings.worldWidth, settings.worldHeight,
            settings.numVoronoiCells,
            settings.seed);
        _regionIndexMap = NoiseGenerator.GenerateVoronoiRegionMap(
            settings.worldWidth, settings.worldHeight,
            settings.numVoronoiCells, settings.seed, out var cellPoints);

        var worldCenter = new Vector2(settings.worldWidth / 2f, settings.worldHeight / 2f);
        var maxRadius = Mathf.Min(settings.worldWidth, settings.worldHeight) / 2f * 0.8f;
        _biomeAssignments = new BiomeData[settings.numVoronoiCells];
        foreach (var cellPoint in cellPoints) Debug.Log(cellPoint);
        for (var i = 0; i < cellPoints.Length; i++)
        {
            var dist = Vector2.Distance(cellPoints[i], worldCenter);


            if (dist > maxRadius)
                _biomeAssignments[i] = settings.defaultBiome;
            else

                _biomeAssignments[i] = settings.allBiomes[Random.Range(0, settings.allBiomes.Length)];
        }

        _biomeMap = new BiomeData[settings.worldWidth, settings.worldHeight];

        for (var y = 0; y < settings.worldHeight; y++)
        for (var x = 0; x < settings.worldWidth; x++)
        {
            var assignedBiome = DetermineBiome(x, y);
            _biomeMap[x, y] = assignedBiome;

            InstantiateGroundTile(x, y, assignedBiome);

            InstantiateObjects(x, y, assignedBiome);
        }

        Debug.Log("World Generation Complete!");
    }

    private void ClearExistingWorld()
    {
        if (worldContainer != null)
            for (var i = worldContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(worldContainer.GetChild(i).gameObject);
    }


    private BiomeData DetermineBiome(int x, int y)
    {
        var regionIndex = _regionIndexMap[x, y];
        return _biomeAssignments[regionIndex];
    }

    private void InstantiateGroundTile(int x, int y, BiomeData biome)
    {
        var tilePrefab = biome.GetRandomGroundTile();
        var xOffset = settings.worldWidth * settings.tileSize / 2f;
        var zOffset = settings.worldHeight * settings.tileSize / 2f;
        var tileSizeVector = new Vector3(settings.tileSize, settings.tileSize, 1);

        if (tilePrefab != null)
        {
            var spawnPosition = new Vector3(x * settings.tileSize - xOffset, 0, y * settings.tileSize - zOffset);

            var newTile = Instantiate(tilePrefab, spawnPosition, tilePrefab.transform.rotation);
            newTile.transform.localScale = tileSizeVector;
            newTile.transform.parent = worldContainer;
            newTile.name = $"Tile_{x}_{y}_{biome.biomeName}";
        }
    }


    private void InstantiateObjects(int x, int y, BiomeData biome)
    {
        var objectsToSpawn = biome.GetObjectsToSpawn();
        if (objectsToSpawn == null) return;

        var worldOffsetX = settings.worldWidth * settings.tileSize / 2f;
        var worldOffsetZ = settings.worldHeight * settings.tileSize / 2f;

        foreach (var objPrefab in objectsToSpawn)
        {
            var tileOffsetX = (Random.value - 0.5f) * settings.tileSize;
            var tileOffsetZ = (Random.value - 0.5f) * settings.tileSize;

            var baseX = x * settings.tileSize - worldOffsetX;
            var baseZ = y * settings.tileSize - worldOffsetZ;

            var spawnPosition = new Vector3(baseX + tileOffsetX, 0f, baseZ + tileOffsetZ);

            var newObject = Instantiate(objPrefab, spawnPosition, Quaternion.identity);
            newObject.transform.parent = worldContainer;
            newObject.name = $"{objPrefab.name}_{x}_{y}";
        }
    }
}