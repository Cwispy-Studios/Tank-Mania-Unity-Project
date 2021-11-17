using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [Serializable]
  public abstract class Upgrader
  {
    [SerializeField] private StatModifier statModifier;
    public StatModifier StatModifier => statModifier;

    public abstract void UpgradeUpgrader();
  }

}
