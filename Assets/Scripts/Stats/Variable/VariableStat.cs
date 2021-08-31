using System;
using System.Collections.Generic;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [Serializable]
  public abstract class VariableStat
  {
    // The list of stat modifiers that will affect this stat's upgraded value
    public List<StatModifier> StatModifiers = new List<StatModifier>();

    // Event when a stat modifier that this stat is subscribed to is upgraded
    public Action OnStatUpgrade;

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

    public abstract void SetDefaultUpgradedValue();

    public abstract void RecalculateStat();
  }
}
