using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public class Stat : ScriptableObject
  {
    // Event when a stat modifier that this stat is subscribed to is upgraded
    public event Action OnStatUpgrade;

    [SerializeField] private bool useInt = false;
    [SerializeField] private float baseValue;

    private float upgradedValue;
    public float Value => useInt ? Mathf.RoundToInt(upgradedValue) : upgradedValue;
    public int IntValue => Mathf.RoundToInt(upgradedValue);

    [NonSerialized] private float totalAdditiveValue = 0f;
    [NonSerialized] private float totalMulitplicativeValue = 1f;

    private void Awake()
    {
      SetDefaultUpgradedValue();
    }

    private void OnValidate()
    {
      // Does not need to be called in build since the values get serialized in editor already
      SetDefaultUpgradedValue();
    }

    private void SetDefaultUpgradedValue()
    {
      upgradedValue = baseValue;
    }

    public void AdjustUpgradeValues( float additiveValue, float multiplicativeValue )
    {
      totalAdditiveValue += additiveValue;
      totalMulitplicativeValue += multiplicativeValue;

      upgradedValue = (baseValue + totalAdditiveValue) * totalMulitplicativeValue;

      OnStatUpgrade?.Invoke();
    }
  }
}
