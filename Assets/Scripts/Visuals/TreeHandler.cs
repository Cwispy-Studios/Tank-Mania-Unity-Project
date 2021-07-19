using System;
using System.Collections.Generic;
using CwispyStudios.TankMania.Projectile;
using UnityEngine;

namespace Visuals
{
  public class TreeHandler : MonoBehaviour
  {
    private List<Projectile> bulletTransforms = new List<Projectile>();
    private Tree[] trees;

    private void Awake()
    {
      BulletEvents.onBulletFired += AddBullet;
      BulletEvents.onBulletHit += RemoveBullet;

      InitialiseTrees();
    }

    private void AddBullet(Projectile bullet)
    {
      bulletTransforms.Add(bullet);
    }

    private void RemoveBullet(Projectile bullet)
    {
      bulletTransforms.Remove(bullet);
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
  }
}
