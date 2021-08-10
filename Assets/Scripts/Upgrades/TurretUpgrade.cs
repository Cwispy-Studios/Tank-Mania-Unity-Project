using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Turret Upgrade")]
  public class TurretUpgrade : Upgrade
  {
    [Header("Tank Turret Modifiers")]
    [Tooltip("How much damage is dealt on a direct hit.")]
    public FloatModifier Damage;
    [Tooltip("How fast the projectile travels, affects the flatness of its trajectory.")]
    public FloatModifier ProjectileSpeed;
    [Tooltip("How fast the ammunition is reloaded in seconds.")]
    public FloatModifier ReloadSpeed;
    [Tooltip("Interval in seconds between firing each ammunition without reloading.")]
    public FloatModifier FireRate;
    [Tooltip("Radius of the splash damage.")]
    public FloatModifier AoeRadius;
    [Tooltip("Damage modifier of the splash damage from the direct damage.")]
    public FloatModifier AoeDamage;

    [Tooltip("How much ammunition the tank can carry.")]
    [Range(-2, 2)] public int Ammunition;

    public static TurretUpgrade operator +( TurretUpgrade t1, TurretUpgrade t2 )
    {
      TurretUpgrade combinedUpgrade = CreateInstance<TurretUpgrade>();

      combinedUpgrade.Damage = t1.Damage + t2.Damage;
      combinedUpgrade.ProjectileSpeed = t1.ProjectileSpeed + t2.ProjectileSpeed;
      combinedUpgrade.ReloadSpeed = t1.ReloadSpeed + t2.ReloadSpeed;
      combinedUpgrade.FireRate = t1.FireRate + t2.FireRate;
      combinedUpgrade.AoeRadius = t1.AoeRadius + t2.ProjectileSpeed;
      combinedUpgrade.AoeDamage = t1.AoeDamage + t2.AoeDamage;
      combinedUpgrade.Ammunition = t1.Ammunition + t2.Ammunition;

      return combinedUpgrade;
    }
  }
}
