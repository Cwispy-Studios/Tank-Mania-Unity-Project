using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [System.Serializable]
  public class FloatStat : VariableStat
  {
    [SerializeField] private float baseValue;

    private float upgradedValue;
    public float Value => upgradedValue;

    /// <summary>
    /// Sets the default value in the inspector
    /// </summary>
    /// <param name="baseValue"></param>
    public FloatStat( float baseValue )
    {
      this.baseValue = baseValue;
    }

    public override void SetDefaultUpgradedValue()
    {
      upgradedValue = baseValue;
    }

    public override void RecalculateStat()
    {
      SetDefaultUpgradedValue();

      float totalAdditiveValue = 0f;
      float totalMultiplicativeValue = 0f;

      foreach (UpgradeSubscription upgradeSubscription in UpgradeSubscriptions)
      {
        if (!upgradeSubscription.IsValid) continue;

        StatModifierInstance statModifierInstance = upgradeSubscription.StatModifierInstance;
        StatModifier statModifier = statModifierInstance.StatModifier;

        int upgradedAmount = statModifierInstance.UpgradedAmount;

        totalAdditiveValue += statModifier.AddititiveValue * upgradedAmount;
        totalMultiplicativeValue += statModifier.MultiplicativeValue * upgradedAmount;
      }

      upgradedValue += totalAdditiveValue;
      upgradedValue *= 1f + totalMultiplicativeValue;

      OnStatUpgrade?.Invoke();
    }
  }
}
