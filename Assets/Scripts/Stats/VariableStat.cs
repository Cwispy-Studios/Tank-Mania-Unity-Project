using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [System.Serializable]
  public abstract class VariableStat
  {
    public List<StatModifier> StatModifiers = new List<StatModifier>();

    public void SubscribeToStatModifiers()
    {
      foreach (StatModifier statModifier in StatModifiers) statModifier.OnStatUpgrade += RecalculateStat;
    }

    public void UnsubscribeFromStatModifiers()
    {
      foreach (StatModifier statModifier in StatModifiers) statModifier.OnStatUpgrade -= RecalculateStat;
    }

    public abstract void RecalculateStat();
  }
}
