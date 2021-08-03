using System;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public class SplashDamageInformation
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

    public float SplashRadius;
    public float SplashDamagePercentage;

    public bool HasSplashDamageRolloff;
    public float MinRadiusPercentageRolloff;
    public float MaxRadiusPercentageRolloff;
    public float MinRadiusDamagePercentageRolloff;
    public float MaxRadiusDamagePercentageRolloff;

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
  }
}
