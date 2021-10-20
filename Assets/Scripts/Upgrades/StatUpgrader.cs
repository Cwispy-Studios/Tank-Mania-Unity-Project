using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [Serializable]
  public class StatUpgrader
  {
    [SerializeField] private Stat statUpgraded;
    public Stat StatUpgraded => statUpgraded;

    [SerializeField] private StatModifier statModifier;
    public StatModifier StatModifier => statModifier;

    private int upgradedAmount = 0;
    public int UpgradedAmount => upgradedAmount;

    public void UpgradeStatUpgrader()
    {
      ++upgradedAmount;
      statUpgraded.AdjustUpgradeValues(statModifier.AddititiveValue, statModifier.MultiplicativeValue);
    }
  }
}
