using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [Serializable]
  public class StatModifier
  {
    [SerializeField] private float additiveValue;
    public float AddititiveValue => additiveValue;

    [SerializeField] private float multiplicativeValue;
    public float MultiplicativeValue => multiplicativeValue;

#if UNITY_EDITOR
    [SerializeField] private bool useInt;
#endif
  }
}
