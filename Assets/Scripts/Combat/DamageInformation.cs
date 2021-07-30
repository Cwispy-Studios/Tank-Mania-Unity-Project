using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public class DamageInformation
  {
    [Range(0f, 10000f)] public float DirectDamage;
    [HideInInspector] public Team DamageFrom;

    public SplashDamageInformation SplashDamage;
  }
}
