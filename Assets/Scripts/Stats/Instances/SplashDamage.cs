using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [System.Serializable]
  public class SplashDamage
  {
    public bool HasSplashDamage;

    [Tooltip("Radius of the splash damage.")]
    public FloatStat Radius = new FloatStat(2f);
    [Tooltip("Damage modifier of the splash damage from the direct damage.")]
    [StatPercentage] public FloatStat DamagePercentage = new FloatStat(1f);

    public bool HasSplashDamageRolloff;
    [Header("Radius Cutoff")]
    [StatPercentage, Tooltip("Percentage distance from the original radius the rolloff starts.")] 
    public FloatStat MinRadiusPercentageRolloff = new FloatStat(0f);
    [StatPercentage, Tooltip("Percentage distance from the original radius the rolloff ends.")] 
    public FloatStat MaxRadiusPercentageRolloff = new FloatStat(1f);
    [Header("Damage Percentages of Radius")]
    [StatPercentage, Tooltip("Percentage damage dealt where the rolloff starts.")] 
    public FloatStat MinRadiusDamagePercentageRolloff = new FloatStat(1f);
    [StatPercentage, Tooltip("Percentage damage dealt where the rolloff ends.")]
    public FloatStat MaxRadiusDamagePercentageRolloff = new FloatStat(0f);
  }
}
