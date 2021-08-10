using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Upgraded Upgrades")]
  public class UpgradedUpgrades : ScriptableObject
  {
    private Dictionary<Upgrade, int> upgradedUpgrades = new Dictionary<Upgrade, int>();

    public Action<Upgrade> onUpgradeEvent;

    public void Upgrade( Upgrade upgrade )
    {
      if (upgradedUpgrades.ContainsKey(upgrade))
      {
        upgradedUpgrades.Add(upgrade, 1);
      }

      else
      {
        ++upgradedUpgrades[upgrade];
      }

      onUpgradeEvent?.Invoke(upgrade);
    }

    private void CombineUpgrades()
    {

    }
  }
}
