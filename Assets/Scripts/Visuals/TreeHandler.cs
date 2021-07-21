using System;
using System.Collections.Generic;
using CwispyStudios.TankMania.Projectile;
using UnityEngine;
using Tree = UnityEngine.Tree;

namespace CwispyStudios.TankMania.Visuals
{
  public class TreeHandler : MonoBehaviour
  {
    private List<Projectile.Projectile> bulletTransforms = new List<Projectile.Projectile>();
    private Tree[] trees;

    private void Awake()
    {
      BulletEvents.OnBulletFired += AddBullet;
      BulletEvents.OnBulletHit += RemoveBullet;

      InitialiseTrees();
    }

    private void AddBullet(Projectile.Projectile bullet)
    {
      bulletTransforms.Add(bullet);
      ApplyForceTrees(bullet);
    }

    private void RemoveBullet(Projectile.Projectile bullet)
    {
      bulletTransforms.Remove(bullet);
      ApplyForceTrees(bullet);
    }

    private void InitialiseTrees()
    {
      trees = FindObjectsOfType<Tree>();
    }

    private void Update()
    {
      foreach (var tree in trees)
      {
        tree.UpdateTree();
      }
    }

    private void ApplyForceTrees(Projectile.Projectile bullet)
    {
      foreach (var tree in trees)
      {
        tree.AddForce(bullet.transform.position); 
      }
    }
  }
}
