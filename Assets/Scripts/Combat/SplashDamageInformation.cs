using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public class SplashDamageInformation
  {
    public bool HasSplashDamage;

    [Range(0f, 5f)] public float SplashRadius;
    [Range(0f, 1f)] public float SplashDamagePercentage;

    [Header("Splash Damage Rolloff")]
    public bool HasSplashDamageRolloff;
    [Range(0f, 1f)] public float MinRadiusPercentageRolloff;
    [Range(0f, 1f)] public float MaxRadiusPercentageRolloff;
    [Range(0f, 1f)] public float MinRadiusDamagePercentageRolloff;
    [Range(0f, 1f)] public float MaxRadiusDamagePercentageRolloff;
  }
}
