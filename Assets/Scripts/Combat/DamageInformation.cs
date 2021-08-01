using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public class DamageInformation
  {
    [SerializeField, Range(0f, 10000f)] private float directDamage;
    public float DirectDamage
    {
      get { return directDamage; }
      set
      {
        directDamage = value;

        // Update cached information in splash damage
        if (SplashDamageInformation.HasSplashDamage) ;
      }
    }

    [HideInInspector] public Team DamageFrom;

    public SplashDamageInformation SplashDamageInformation;
  }
}
