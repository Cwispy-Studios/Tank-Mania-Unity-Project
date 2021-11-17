using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [Serializable]
  public class StatUpgrader : Upgrader
  {
    [SerializeField] private Stat statUpgraded;
    public Stat StatUpgraded => statUpgraded;

    public override void UpgradeUpgrader()
    {
      statUpgraded.AdjustUpgradeValues(StatModifier.AddititiveValue, StatModifier.MultiplicativeValue);
    }
  }
}
