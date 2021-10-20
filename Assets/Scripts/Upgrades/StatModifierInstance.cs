using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [Serializable]
  public class StatModifierInstance
  {
    [SerializeField] private string instanceName;
    public string InstanceName => instanceName;

    [SerializeField] private StatModifier statModifier;
    public StatModifier StatModifier => statModifier;

    [SerializeField] private StatSubscription[] statSubscriptions;

    private int upgradedAmount = 0;
    public int UpgradedAmount => upgradedAmount;

    public event Action OnUpgradeEvent;

    public void UpgradeStatModifierInstance()
    {
      ++upgradedAmount;
      OnUpgradeEvent?.Invoke();
    }
  }
}
