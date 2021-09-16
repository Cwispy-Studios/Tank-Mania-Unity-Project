using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
  using Combat;

  public class TreeHandler : MonoBehaviour
  {
    private List<Projectile> bulletTransforms = new List<Projectile>();
    private Tree[] trees;

    private void Awake()
    {
      BulletEvents.OnBulletFired += AddBullet;
      BulletEvents.OnBulletHit += RemoveBullet;

      InitialiseTrees();
    }

    private void AddBullet(Projectile bullet)
    {
      bulletTransforms.Add(bullet);
      ApplyForceTrees(bullet);
    }

    private void RemoveBullet(Projectile bullet)
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

    private void ApplyForceTrees(Projectile bullet)
    {
      foreach (var tree in trees)
      {
        tree.AddForce(bullet.transform.position); 
      }
    }
  }
}
