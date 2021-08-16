using System.Collections.Generic;

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

      RecalculateStat();
    }

    //public void SubscribeToStatModifiers( List<StatModifier> modifiersList )
    //{
    //  StatModifiers.AddRange(modifiersList);

    //  foreach (StatModifier statModifier in modifiersList) statModifier.OnStatUpgrade += RecalculateStat;

    //  RecalculateStat();
    //}

    public void UnsubscribeFromStatModifiers()
    {
      foreach (StatModifier statModifier in StatModifiers) statModifier.OnStatUpgrade -= RecalculateStat;
    }

    //public void UnsubscribeFromStatModifiers( List<StatModifier> modifiersList )
    //{
    //  foreach (StatModifier statModifier in modifiersList) statModifier.OnStatUpgrade -= RecalculateStat;
    //}

    public abstract void SetDefaultUpgradedValue();

    public abstract void RecalculateStat();
  }
}
