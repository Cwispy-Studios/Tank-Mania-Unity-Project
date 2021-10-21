using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [Serializable]
  public class StatsCategoryUpgrader : Upgrader
  {
    [SerializeField] private StatsCategory statsCategoryUpgraded;
    public StatsCategory StatsCategoryUpgraded => statsCategoryUpgraded;

    public override void UpgradeUpgrader()
    {
      foreach (Stat stat in statsCategoryUpgraded.StatsInCategory)
      {
        stat.AdjustUpgradeValues(StatModifier.AddititiveValue, StatModifier.MultiplicativeValue);
      }
    }
  }
}
