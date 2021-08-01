using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public class SplashDamageInformation
  {
    public bool HasSplashDamage;

    public float SplashRadius;
    public float SplashDamagePercentage;

    [Header("Splash Damage Rolloff")]
    public bool HasSplashDamageRolloff;
    public float MinRadiusPercentageRolloff;
    public float MaxRadiusPercentageRolloff;
    public float MinRadiusDamagePercentageRolloff;
    public float MaxRadiusDamagePercentageRolloff;
  }
}
