using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [Serializable]
  public class StatModifierInstance
  {
    [SerializeField] private string instanceName;
    public string InstanceName => instanceName;

    [SerializeField] private StatModifier statModifier;
    public StatModifier StatModifier => statModifier;

    private int upgradedAmount = 0;
    public int UpgradedAmount => upgradedAmount;

    public event Action OnUpgradeEvent;

    public void UpgradeStatModifierInstance()
    {
      ++upgradedAmount;
      OnUpgradeEvent?.Invoke();
    }

#if UNITY_EDITOR
    public Action OnInstanceRemoved;
#endif
  }
}
