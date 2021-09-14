using System;
using CwispyStudios.TankMania.Stats;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  [Serializable]
  public class AttackInformation
  {
    [Header("Attach transform as child of main object as hit box")]
    public Transform hitBox;

    [Header("Stats")]
    public Damage damageStats;
  }
}