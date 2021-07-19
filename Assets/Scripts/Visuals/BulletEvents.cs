using System;
using CwispyStudios.TankMania.Projectile;
using UnityEngine;

namespace Visuals
{
    public class BulletEvents : MonoBehaviour
    {
        public static event Action<Projectile> onBulletFired;

        public static void BulletFired(Projectile projectile)
        {
            onBulletFired?.Invoke(projectile);
        }
        
        public static event Action<Projectile> onBulletHit;

        public static void BulletHit(Projectile projectile)
        {
            onBulletHit?.Invoke(projectile);
        }
    }
}
