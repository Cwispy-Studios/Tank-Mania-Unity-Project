using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  public class IntStat : VariableStat
  {
    [SerializeField] private int baseValue;

    private int upgradedValue;

    public int Value => upgradedValue;

    /// <summary>
    /// Sets the default value in the inspector
    /// </summary>
    /// <param name="baseValue"></param>
    public IntStat( int baseValue )
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

      float totalAdditiveValue = 0;
      float totalMultiplicativeValue = 0;

      foreach (StatModifier statModifier in StatModifiers)
      {
        int upgradedAmount = statModifier.UpgradedAmount;

        totalAdditiveValue += statModifier.AddititiveValue * upgradedAmount;
        totalMultiplicativeValue += statModifier.MultiplicativeValue * upgradedAmount;
      }

      upgradedValue += Mathf.RoundToInt(totalAdditiveValue);
      upgradedValue = Mathf.RoundToInt(upgradedValue * (1f + totalMultiplicativeValue));
    }
  }
}