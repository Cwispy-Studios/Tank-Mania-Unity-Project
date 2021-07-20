using System;
using CwispyStudios.TankMania.Projectile;
using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
    public class BulletEvents : MonoBehaviour
    {
        public static event Action<Projectile.Projectile> OnBulletFired;

        public static void BulletFired(Projectile.Projectile projectile)
        {
            OnBulletFired?.Invoke(projectile);
        }
        
        public static event Action<Projectile.Projectile> OnBulletHit;

        public static void BulletHit(Projectile.Projectile projectile)
        {
            OnBulletHit?.Invoke(projectile);
        }
    }
}
