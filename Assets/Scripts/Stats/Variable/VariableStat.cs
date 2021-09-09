using System;
using System.Collections.Generic;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [Serializable]
  public abstract class VariableStat
  {
    // The list of stat modifiers that will affect this stat's upgraded value
    public List<UpgradeSubscription> UpgradeSubscriptions = new List<UpgradeSubscription>();

    // Event when a stat modifier that this stat is subscribed to is upgraded
    public Action OnStatUpgrade;

    public void SubscribeToUpgradeInstances()
    {
      // List is null when this function is called
      if (UpgradeSubscriptions == null) UpgradeSubscriptions = new List<UpgradeSubscription>();

      foreach (UpgradeSubscription upgradeSubscription in UpgradeSubscriptions)
      {
        if (upgradeSubscription.IsValid)
          upgradeSubscription.StatModifierInstance.OnUpgradeEvent += RecalculateStat;
      }

      // Ensures the upgraded value is initialised and returns a valid value
      RecalculateStat();
    }

    public void UnsubscribeFromUpgradeInstances()
    {
      foreach (UpgradeSubscription upgradeSubscription in UpgradeSubscriptions)
      {
        if (upgradeSubscription.IsValid)
          upgradeSubscription.StatModifierInstance.OnUpgradeEvent -= RecalculateStat;
      }
    }

    public abstract void SetDefaultUpgradedValue();

    public abstract void RecalculateStat();
  }
}
