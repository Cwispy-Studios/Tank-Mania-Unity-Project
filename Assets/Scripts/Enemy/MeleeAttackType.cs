using System;
using CwispyStudios.TankMania.Stats;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  [Serializable]
  public class MeleeAttackType
  {
    [Header("Attach transform as child of main object as hit box")]
    public Transform HitBox;

    [Header("Stats")]
    public AttackAttributes AttackAttributes;
  }
}