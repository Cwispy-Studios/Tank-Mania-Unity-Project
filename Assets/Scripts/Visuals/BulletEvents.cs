using System;
using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
  using Combat;
  public class BulletEvents : MonoBehaviour
  {
    public static event Action<Projectile> OnBulletFired;

    public static void BulletFired(Projectile projectile)
    {
      OnBulletFired?.Invoke(projectile);
    }
        
    public static event Action<Projectile> OnBulletHit;

    public static void BulletHit(Projectile projectile)
    {
      OnBulletHit?.Invoke(projectile);
    }
  }
}
