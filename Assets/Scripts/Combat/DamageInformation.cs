using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public class DamageInformation
  {
    //private float directDamage;
    public float DirectDamage;
    //{
    //  get { return directDamage; }
    //  set
    //  {
    //    if (directDamage != value)
    //    {
    //      directDamage = value;

    //      // Update cached information in splash damage
    //      if (SplashDamageInformation.HasSplashDamage) ;
    //    }
    //  }
    //}

    [HideInInspector] public Team DamageFrom;

    public SplashDamageInformation SplashDamageInformation;

    //public void Initialise()
    //{
    //  SplashDamageInformation.Initialise(this);
    //}
  }
}
