using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Projectile;
  using Visuals;

  [CreateAssetMenu(menuName = "Player/Firing Information")]
  public class FiringInformation : ScriptableObject
  {
    [Header("Projectile")]
    public Projectile ProjectilePrefab;

    [Header("VFX")]
    public VfxParentDisabler FiringVfx;

    [Header("Firing Stats")]
    [Range(1f, 25f)] public float FiringForce = 20f;
    [Range(0.5f, 3f)] public float IntervalBetweenFiring = 1.5f;
    [Range(0.1f, 0.5f)] public float TimeToQueueFiring = 0.2f;
  }
}
