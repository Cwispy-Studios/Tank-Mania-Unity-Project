using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Combat;
  using Visuals;

  [CreateAssetMenu(menuName = "Stats/Combat/Projectile Attack Attributes")]
  public class ProjectileAttackAttributes : AttackAttributes
  {
    [Header("Projectile"), Tooltip("Projectile that is spawned when attacking.")]
    public Projectile ProjectilePrefab;

    [Header("VFX"), Tooltip("VFX to play when projectile is fired.")]
    public VfxParentDisabler FiringVfx;

    [Header("Firing Attributes")]
    [StatRange(1f, 200f), // 20f
      Tooltip("Physics force applied to projectile and how fast it travels, affects the flatness of its trajectory.")]
    public Stat FiringForce;
    [StatRange(2f, 4f), // 3f
      Tooltip("How fast the ammunition is reloaded in seconds.")]
    public Stat ReloadSpeed;
    [StatRange(0.1f, 0.5f), // 0.2f
      Tooltip("Time in seconds before firing will be queued if the input action is performed.")] 
    public Stat TimeToQueueFiring;
    [StatRange(1, 20), // 5
      Tooltip("Number of ammunition that can be held at once.")]
    public Stat AmmoCount;
  }
}
