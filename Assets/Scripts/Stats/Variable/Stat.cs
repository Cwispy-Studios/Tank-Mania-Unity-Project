using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  public class Stat : ScriptableObject
  {
    // The list of stat modifiers that will affect this stat's upgraded value
    public List<StatModifier> StatModifiers = new List<StatModifier>();

    // Event when a stat modifier that this stat is subscribed to is upgraded
    public Action OnStatUpgrade;

    [SerializeField] private bool useInt = false;
    [SerializeField] private float baseValue;

    private float upgradedValue;
    public float Value => useInt ? Mathf.RoundToInt(upgradedValue) : upgradedValue;
    public int IntValue => Mathf.RoundToInt(upgradedValue);

    private float totalAdditiveValue = 0f;
    private float totalMulitplicativeValue = 1f;

    public Stat( bool valueAsInteger )
    {
      useInt = valueAsInteger;
    }

    public Stat( int value )
    {
      useInt = true;
      baseValue = value;
    }

    public Stat( float value )
    {
      useInt = false;
      baseValue = value;
    }

    public void SubscribeToStatModifiers()
    {
      // List is null when this function is called
      if (StatModifiers == null) StatModifiers = new List<StatModifier>();

      foreach (StatModifier statModifier in StatModifiers) statModifier.OnStatUpgrade += RecalculateStat;

      // Ensures the upgraded value is initialised and returns a valid value
      RecalculateStat();
    }

    public void UnsubscribeFromStatModifiers()
    {
      foreach (StatModifier statModifier in StatModifiers) statModifier.OnStatUpgrade -= RecalculateStat;
    }

    public void SetDefaultUpgradedValue()
    {
      upgradedValue = baseValue;
    }

    public void RecalculateStat()
    {
      SetDefaultUpgradedValue();

      float totalAdditiveValue = 0f;
      float totalMultiplicativeValue = 1f;

      foreach (StatModifier statModifier in StatModifiers)
      {
        //int upgradedAmount = statModifier.UpgradedAmount;

        //totalAdditiveValue += statModifier.AddititiveValue * upgradedAmount;
        //totalMultiplicativeValue += statModifier.MultiplicativeValue * upgradedAmount;
      }

      upgradedValue += totalAdditiveValue;
      upgradedValue *= totalMultiplicativeValue;

      OnStatUpgrade?.Invoke();
    }
  }
}
