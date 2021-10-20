using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [Serializable]
  public class StatModifier
  {
    [SerializeField] private float additiveValue;
    public float AddititiveValue => additiveValue;

    [SerializeField] private float multiplicativeValue = 1f;
    public float MultiplicativeValue => multiplicativeValue;
  }
}
