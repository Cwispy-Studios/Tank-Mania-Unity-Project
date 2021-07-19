using System.Collections.Generic;

using UnityEngine;
using Visuals;

namespace CwispyStudios.TankMania.Projectile
{
  public class ProjectilePooler : MonoBehaviour
  {
    private const int NumberPooledPerPrefab = 50;

    [SerializeField] private List<Projectile> projectilePrefabsList = null;

    private Dictionary<string, List<Projectile>> pooledProjectilesDictionary;

    private void Awake()
    {
      InitialiseProjectilePooler();
    }

    private void InitialiseProjectilePooler()
    {
      pooledProjectilesDictionary = new Dictionary<string, List<Projectile>>();

      foreach (Projectile projectilePrefab in projectilePrefabsList)
      {
        if (projectilePrefab != null)
        {
          List<Projectile> projectileList = new List<Projectile>();

          for (int i = 0; i < NumberPooledPerPrefab; ++i)
          {
            Projectile projectile = InstantiateProjectile(projectilePrefab, i);
            projectileList.Add(projectile);
          }

          pooledProjectilesDictionary.Add(projectilePrefab.name, projectileList);
        }
      }
    }

    private Projectile InstantiateProjectile( Projectile projectilePrefab, int number )
    {
      Projectile projectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity, transform);
      projectile.gameObject.SetActive(false);
      projectile.gameObject.name = projectilePrefab.name + number;

      return projectile;
    }

    private Projectile FindInactiveProjectile( Projectile projectilePrefab )
    {
      List<Projectile> projectileList = pooledProjectilesDictionary[projectilePrefab.name];

      // Find an inactive projectile in the list
      for (int i = 0; i < projectileList.Count; ++i)
      {
        if (!projectileList[i].gameObject.activeSelf)
        {
          return projectileList[i];
        }
      }

      // If we reach here, there are no more inactive projectiles and we need to instantiate one
      Projectile projectile = InstantiateProjectile(projectilePrefab, projectileList.Count);
      projectileList.Add(projectile);

      return projectile;
    }

    public Projectile EnableProjectile( Projectile projectilePrefab, Vector3 spawnLocation, Quaternion rotation )
    {
      Projectile projectile = FindInactiveProjectile(projectilePrefab);
      projectile.transform.position = spawnLocation;
      projectile.transform.rotation = rotation;
      projectile.gameObject.SetActive(true);

      BulletEvents.BulletFired(projectile);

      return projectile;
    }
  }
}
