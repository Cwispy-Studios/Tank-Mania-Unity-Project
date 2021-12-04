using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Damage")]
  public class Damage : StatsGroup
  {
    [Header("Damage Information"), Tooltip("How much damage is dealt on a direct hit.")]
    public Stat DirectDamage; // 10f

    [StatPercentage, Tooltip("Percentage damage dealt of a dud collision")]
    public Stat DudCollisionDamagePercentage;

    // Whether damage is splash damage, which affects if below variables are ever used
    [Tooltip("Is splash damage enabled?")]
    public bool HasSplashDamage;

    [Tooltip("Radius of the splash damage.")]
    public Stat SplashRadius; // 2f
    [Tooltip("Damage modifier of the splash damage from the direct damage.")]
    [StatPercentage] public Stat SplashDamagePercentage; // 1f

    public bool HasSplashDamageRolloff;
    [Header("Radius Cutoff")]
    [StatPercentage, Tooltip("Percentage distance from the original radius the rolloff starts.")]
    public Stat SplashMinRadiusPercentageRolloff; // 0f
    [StatPercentage, Tooltip("Percentage distance from the original radius the rolloff ends.")]
    public Stat SplashMaxRadiusPercentageRolloff; // 1f
    [Header("Damage Percentages of Radius")]
    [StatPercentage, Tooltip("Percentage damage dealt where the rolloff starts.")]
    public Stat SplashMinRadiusDamagePercentageRolloff; // 1f
    [StatPercentage, Tooltip("Percentage damage dealt where the rolloff ends.")]
    public Stat SplashMaxRadiusDamagePercentageRolloff; // 0f
  }
}
