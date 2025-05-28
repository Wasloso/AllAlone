using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName = "World/Biome")]
public class BiomeData : ScriptableObject
{
    public string biomeName;

    [Tooltip("The primary ground tile for this biome.")]
    public GameObject primaryGroundPrefab;

    [Tooltip("Additional ground tiles for variation, with their relative weights.")]
    public BiomeGroundTile[] additionalGroundTiles;

    [Header("Objects")] [Tooltip("Objects that can spawn in this biome, with their spawn chances.")]
    public BiomeObjectSpawn[] objectSpawns;

    [Tooltip("The elevation range (normalized 0-1) at which this biome typically appears.")] [MinMaxSlider(0f, 1f)]
    public Vector2 elevationRange = new(0f, 1f);

    [Tooltip("The temperature range (normalized 0-1) at which this biome typically appears.")] [MinMaxSlider(0f, 1f)]
    public Vector2 temperatureRange = new(0f, 1f);

    [Tooltip("The humidity range (normalized 0-1) at which this biome typically appears.")] [MinMaxSlider(0f, 1f)]
    public Vector2 humidityRange = new(0f, 1f);

    [Tooltip("Influence on the likelihood of rivers spawning in this biome.")] [Range(0f, 1f)]
    public float riverSpawnInfluence = 0.5f;


    private float[] _cumulativeGroundWeights;
    private float _totalGroundWeight;

    public void OnEnable()
    {
        CalculateGroundWeights();
    }

    private void CalculateGroundWeights()
    {
        if (additionalGroundTiles == null || additionalGroundTiles.Length == 0)
        {
            _cumulativeGroundWeights = null;
            _totalGroundWeight = 0;
            return;
        }

        _cumulativeGroundWeights = new float[additionalGroundTiles.Length];
        _totalGroundWeight = 0;

        for (var i = 0; i < additionalGroundTiles.Length; i++)
        {
            _totalGroundWeight += additionalGroundTiles[i].spawnWeight;
            _cumulativeGroundWeights[i] = _totalGroundWeight;
        }
    }

    public GameObject GetRandomGroundTile()
    {
        if (primaryGroundPrefab != null && (additionalGroundTiles == null || additionalGroundTiles.Length == 0))
            return primaryGroundPrefab;


        if (_totalGroundWeight == 0) return primaryGroundPrefab;

        var randomPoint = Random.value * _totalGroundWeight;

        for (var i = 0; i < _cumulativeGroundWeights.Length; i++)
            if (randomPoint < _cumulativeGroundWeights[i])
                return additionalGroundTiles[i].groundPrefab;

        return primaryGroundPrefab;
    }

    public GameObject[] GetObjectsToSpawn()
    {
        if (objectSpawns == null || objectSpawns.Length == 0) return null;

        var spawnedObjects = new List<GameObject>();

        foreach (var objectSpawn in objectSpawns)
            if (Random.value < objectSpawn.spawnChance)
                spawnedObjects.Add(objectSpawn.objectPrefab);

        return spawnedObjects.ToArray();
    }


    [Serializable]
    public class BiomeGroundTile
    {
        public GameObject groundPrefab;

        [Tooltip("Relative weight for spawning this tile. Higher values mean more frequent.")] [Range(0.01f, 10f)]
        public float spawnWeight = 1f;
    }

    [Serializable]
    public class BiomeObjectSpawn
    {
        public GameObject objectPrefab;

        [Tooltip("Chance (0-1) for this object to spawn. This is per-object, not relative.")] [Range(0f, 1f)]
        public float spawnChance = 0.1f;

        [Tooltip("If true, only one of this object type will spawn per chunk/area.")]
        public bool spawnOncePerArea;
    }
}