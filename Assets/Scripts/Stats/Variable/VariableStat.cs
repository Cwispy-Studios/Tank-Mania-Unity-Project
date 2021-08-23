using System;
using System.Collections.Generic;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [System.Serializable]
  public abstract class VariableStat
  {
    public List<StatModifier> StatModifiers = new List<StatModifier>();

    public Action OnStatUpgrade;

    public void SubscribeToStatModifiers()
    {
      if (StatModifiers == null) StatModifiers = new List<StatModifier>();

      foreach (StatModifier statModifier in StatModifiers) statModifier.OnStatUpgrade += RecalculateStat;

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
