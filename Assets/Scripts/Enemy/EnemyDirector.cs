using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using Poolers;

  public class EnemyDirector : MonoBehaviour
  {
    [Header("Spawn Information")]
    [SerializeField] private Enemy[] enemiesToSpawn;
    [SerializeField] private SpawnInterval spawnInterval;
    [SerializeField] private LayerMask spawnLocationLayerMask;

    [Header("Spawn Distances")]
    [SerializeField, Min(0f)] private float minSpawnDistance;
    [SerializeField, Range(25f, 250f)] private float maxSpawnDistance;

    private ObjectPooler objectPooler;
    private Transform playerTransform;

    private float spawnCountdown;

    private void Awake()
    {
      objectPooler = FindObjectOfType<ObjectPooler>();
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

      RandomiseSpawnInterval();
    }

    private void RandomiseSpawnInterval()
    {
      spawnCountdown = Random.Range(spawnInterval.MinIntervalRange, spawnInterval.MaxIntervalRange);
    }

    private void Update()
    {
      spawnCountdown -= Time.deltaTime;

      if (spawnCountdown < 0f) SpawnEnemy();
    }

    private void SpawnEnemy()
    {
      // First, get a spawn location to spawn an enemy at
      Transform spawnLocation = GetRandomSpawnLocationAroundPlayer();

      // Second, get an enemy type to spawn
      Enemy enemyToSpawn = GetRandomEnemyToSpawn();

      objectPooler.EnablePooledObject(enemyToSpawn.gameObject, spawnLocation.position, Quaternion.identity, true);

      RandomiseSpawnInterval();
    }

    private Transform GetRandomSpawnLocationAroundPlayer()
    {
      // Find all spawn locations within the maximum spawn distance from the player
      List<Collider> results = new List<Collider>(
        Physics.OverlapSphere(
          playerTransform.position, 
          maxSpawnDistance, 
          spawnLocationLayerMask, 
          QueryTriggerInteraction.Collide)
      );

      float sqrMinDistance = minSpawnDistance * minSpawnDistance;

      // Filter the found spawn locations based on the minimum distance
      for (int i = results.Count - 1; i >= 0; --i)
      {
        Collider c = results[i];

        float sqrDistance = Vector3.SqrMagnitude(c.transform.position - playerTransform.position);

        if (sqrDistance < sqrMinDistance) results.RemoveAt(i);
      }

      // Randomise a spawn location out of the valid ones
      int randomIndex = Random.Range(0, results.Count);

      return results[randomIndex].transform;
    }

    private Enemy GetRandomEnemyToSpawn()
    {
      int enemyIndex = 0;

      if (enemiesToSpawn.Length > 0)
      {
        enemyIndex = Random.Range(0, enemiesToSpawn.Length);
      }

      return enemiesToSpawn[enemyIndex];
    }
  }
}
