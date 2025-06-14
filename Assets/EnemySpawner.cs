using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public float spawnInterval = 5f;
    public int maxEnemies = 10;
    public float spawnRadius = 30f;
    public float minDistanceFromPlayer = 10f;
    public EnemySpawn[] enemyPrefabs;

    private readonly List<GameObject> activeEnemies = new();
    private Coroutine spawnRoutine;


    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        foreach (var enemy in activeEnemies)
        {
            var distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance > spawnRadius) Destroy(enemy);
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnInterval, spawnInterval * 3.0f));

            if (activeEnemies.Count >= maxEnemies) continue;

            var spawnPos = GetValidSpawnPosition();

            if (spawnPos != Vector3.zero)
            {
                var prefab = GetRandomEnemyPrefab();
                var enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                var autoAllign = enemy.GetComponentInChildren<AutoBottomAlignSprite>();
                if (autoAllign) autoAllign.AlignSpriteToBottom();

                activeEnemies.Add(enemy);

                var health = enemy.GetComponent<HealthSystem>();
                if (health != null)
                    health.OnDied += () => activeEnemies.Remove(enemy);
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        for (var i = 0; i < 10; i++)
        {
            var offset2D = Random.insideUnitCircle.normalized * Random.Range(minDistanceFromPlayer, spawnRadius);
            var pos = player.position + new Vector3(offset2D.x, 0f, offset2D.y);
            return pos; // Always y = 0
        }

        return Vector3.zero;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        var totalWeight = 0f;
        foreach (var spawn in enemyPrefabs)
            totalWeight += spawn.spawnChance;

        var roll = Random.Range(0f, totalWeight);
        var sum = 0f;

        foreach (var spawn in enemyPrefabs)
        {
            sum += spawn.spawnChance;
            if (roll <= sum)
                return spawn.prefab;
        }

        return enemyPrefabs[0].prefab;
    }


    [Serializable]
    public struct EnemySpawn
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float spawnChance;
    }
}