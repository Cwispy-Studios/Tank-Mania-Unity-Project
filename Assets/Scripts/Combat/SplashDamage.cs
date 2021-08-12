using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  [System.Serializable]
  public class SplashDamage
  {
    // Cache to perform calculations on damage
    // CANNOT BE DONE, CAUSES SERIALIZATION DEPTH LIMIT TO EXCEED
    //private DamageInformation damageInformation;
    
    // Whether damage is splash damage, which affects if below variables are ever used
    //private bool hasSplashDamage;
    public bool HasSplashDamage;
    //{
    //  get { return hasSplashDamage; }
    //  set
    //  {
    //    // Only change variable if value is different
    //    if (hasSplashDamage != value)
    //    {
    //      hasSplashDamage = value;

    //      // Only perform caching calculations if splash damage is enabled
    //      if (hasSplashDamage) Initialise();
    //    }
    //  }
    //}

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

    // 
    //public float BaseSplashDamage;

    //public void Initialise( DamageInformation damageInformation )
    //{
    //  this.damageInformation = damageInformation;

    //  if (hasSplashDamage) Initialise();
    //}

    //private void Initialise()
    //{
    //  CalculateBaseSplashDamage();
    //}

    //private void CalculateBaseSplashDamage()
    //{
    //  BaseSplashDamage = damageInformation.DirectDamage * SplashDamagePercentage;
    //}

    //public void Reset()
    //{
    //  SplashDamagePercentage = 1f;

    //  MinRadiusPercentageRolloff = 0f;
    //  MaxRadiusPercentageRolloff = 1f;
    //  MinRadiusDamagePercentageRolloff = 1f;
    //  MaxRadiusDamagePercentageRolloff = 0f;
    //}
  }
}
