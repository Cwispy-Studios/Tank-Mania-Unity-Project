using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  public class Stat : ScriptableObject
  {
    // Event when a stat modifier that this stat is subscribed to is upgraded
    public Action OnStatUpgrade;

    [SerializeField] private bool useInt = false;
    [SerializeField] private float baseValue;

    private float upgradedValue;
    public float Value => useInt ? Mathf.RoundToInt(upgradedValue) : upgradedValue;
    public int IntValue => Mathf.RoundToInt(upgradedValue);

    private float totalAdditiveValue = 0f;
    private float totalMulitplicativeValue = 1f;

    private void OnValidate()
    {
      // Does not need to be called in build since the values get serialized in editor already
      SetDefaultUpgradedValue();
    }

    public void SetDefaultUpgradedValue()
    {
      upgradedValue = baseValue;
    }

    //public void RecalculateStat()
    //{
    //  SetDefaultUpgradedValue();

    //  float totalAdditiveValue = 0f;
    //  float totalMultiplicativeValue = 1f;

    //  foreach (StatModifier statModifier in StatModifiers)
    //  {
    //    //int upgradedAmount = statModifier.UpgradedAmount;

    //    //totalAdditiveValue += statModifier.AddititiveValue * upgradedAmount;
    //    //totalMultiplicativeValue += statModifier.MultiplicativeValue * upgradedAmount;
    //  }

    //  upgradedValue += totalAdditiveValue;
    //  upgradedValue *= totalMultiplicativeValue;

    //  OnStatUpgrade?.Invoke();
    //}
  }
}
