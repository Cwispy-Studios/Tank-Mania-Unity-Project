using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using Combat;
  using Poolers;

  public class Spawner : MonoBehaviour
  {
    [SerializeField] private Damageable[] enemiesToSpawn;
    [SerializeField] private SpawnInterval spawnInterval;

    private EnemyPooler enemyPooler;

    private float spawnCountdown;

    private void Awake()
    {
      enemyPooler = FindObjectOfType<EnemyPooler>();

      RandomiseSpawnInterval();
    }

    private void Update()
    {
      spawnCountdown -= Time.deltaTime;

      if (spawnCountdown < 0f) SpawnEnemy();
    }

    private void SpawnEnemy()
    {
      Damageable enemyToSpawn;

      if (enemiesToSpawn.Length > 0)
      {
        enemyToSpawn = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)];
      }

      else enemyToSpawn = enemiesToSpawn[0];

      enemyPooler.EnablePooledObject(enemyToSpawn, transform.position, Quaternion.identity);
      RandomiseSpawnInterval();
    }

    private void RandomiseSpawnInterval()
    {
      spawnCountdown = Random.Range(spawnInterval.MinIntervalRange, spawnInterval.MaxIntervalRange);
    }
  }
}
