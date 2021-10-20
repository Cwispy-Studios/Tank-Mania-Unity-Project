using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Combat;

  [CreateAssetMenu(menuName = "Stats/Damage")]
  public class Damage : StatsGroup
  {
    [HideInInspector]
    public Team DamageFrom;

    [Header("Damage Information"), Tooltip("How much damage is dealt on a direct hit.")]
    public Stat DirectDamage; // 10f

    [HideInInspector]
    public SplashDamage SplashDamage = new SplashDamage();
  }
}
