using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  [CreateAssetMenu(menuName = "Stats/Damage")]
  public class Damage : StatsGroup
  {
    [HideInInspector]
    public Team DamageFrom;

    [Header("Damage Information"), Tooltip("How much damage is dealt on a direct hit.")]
    public FloatStat DirectDamage = new FloatStat(10f);

    [HideInInspector]
    public SplashDamage SplashDamage;

    public override void SubscribeStats()
    {
      DirectDamage.SubscribeToStatModifiers();

      SplashDamage.SubscribeStats();
    }

    public override void UnsubscribeStats()
    {
      DirectDamage.UnsubscribeFromStatModifiers();

      SplashDamage.UnsubscribeStats();
    }
  }
}
