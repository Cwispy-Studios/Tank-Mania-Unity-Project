using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Projectile;
  using Visuals;

  [CreateAssetMenu(menuName = "Player/Firing Information")]
  public class FiringInformation : StatsGroup
  {
    [Header("Projectile")]
    public Projectile ProjectilePrefab;

    [Header("VFX")]
    public VfxParentDisabler FiringVfx;

    [Header("Firing Stats")]
    [StatRange(1f, 25f), Tooltip("Physics force applied to projectile and how fast it travels, affects the flatness of its trajectory.")] 
    public FloatStat FiringForce = new FloatStat(20f);
    [StatRange(0.5f, 3f), Tooltip("Interval in seconds between firing each ammunition without reloading.")] 
    public FloatStat FireRate = new FloatStat(1.5f);
    [StatRange(2f, 4f), Tooltip("How fast the ammunition is reloaded in seconds.")]
    public FloatStat ReloadSpeed = new FloatStat(3f);
    [StatRange(0.1f, 0.5f), Tooltip("Time in seconds before firing will be queued if the input action is performed.")] 
    public FloatStat TimeToQueueFiring = new FloatStat(0.2f);
    [StatRange(1, 10), Tooltip("Number of ammunition that can be held at once.")]
    public IntStat AmmoCount = new IntStat(4);
  }
}
