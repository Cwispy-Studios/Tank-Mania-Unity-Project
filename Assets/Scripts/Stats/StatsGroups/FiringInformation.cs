using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Combat;
  using Visuals;

  [CreateAssetMenu(menuName = "Stats/Player/Firing Information")]
  public class FiringInformation : StatsGroup
  {
    [Header("Projectile")]
    public Projectile ProjectilePrefab;

    [Header("VFX")]
    public VfxParentDisabler FiringVfx;

    [Header("Firing Stats")]
    [StatRange(1f, 25f), // 20f
      Tooltip("Physics force applied to projectile and how fast it travels, affects the flatness of its trajectory.")]
    public Stat FiringForce;
    [StatRange(0.5f, 3f), // 1.5f
      Tooltip("Interval in seconds between firing each ammunition without reloading.")] 
    public Stat FireRate;
    [StatRange(2f, 4f), // 3f
      Tooltip("How fast the ammunition is reloaded in seconds.")]
    public Stat ReloadSpeed;
    [StatRange(0.1f, 0.5f), // 0.2f
      Tooltip("Time in seconds before firing will be queued if the input action is performed.")] 
    public Stat TimeToQueueFiring;
    [StatRange(1, 10), // 5
      Tooltip("Number of ammunition that can be held at once.")]
    public Stat AmmoCount;
  }
}
