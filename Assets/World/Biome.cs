using UnityEngine;

[CreateAssetMenu(menuName = "World/Biome")]
public class BiomeData : ScriptableObject
{
    public string biomeName;

    [Header("Ground Tiles")] public GameObject[] groundPrefabs;

    [Header("Objects")] public GameObject[] objectPrefabs;

    public float[] objectSpawnChances;

    public GameObject GetRandomGroundTile()
    {
        return groundPrefabs[Random.Range(0, groundPrefabs.Length)];
    }

    public GameObject GetRandomObject()
    {
        for (var i = 0; i < objectPrefabs.Length; i++)
            if (Random.value < objectSpawnChances[i])
                return objectPrefabs[i];
        return null;
    }
}