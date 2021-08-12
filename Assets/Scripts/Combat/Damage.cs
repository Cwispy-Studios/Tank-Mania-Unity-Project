using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  [CreateAssetMenu(menuName = "Stats/Damage")]
  public class Damage : ScriptableObject
  {
    [HideInInspector]
    public Team DamageFrom;

    [Header("Damage Information"), Tooltip("How much damage is dealt on a direct hit.")]
    public FloatStat DirectDamage = new FloatStat(10f);

    [HideInInspector]
    public SplashDamage SplashDamage;
  }
}
