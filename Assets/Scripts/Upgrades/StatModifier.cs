using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Stat Modifier")]
  public class StatModifier : ScriptableObject
  {
    [NonSerialized] private int upgradedAmount = 0;
    public int UpgradedAmount => upgradedAmount;

    [SerializeField] private float additiveValue;
    public float AddititiveValue => additiveValue;

    [SerializeField] private float multiplicativeValue;
    public float MultiplicativeValue => multiplicativeValue;

    public Action OnStatUpgrade;

    public void Upgrade()
    {
      ++upgradedAmount;

      OnStatUpgrade?.Invoke();
    }

#if UNITY_EDITOR
    [SerializeField] private List<string> statsModified = new List<string>();

    public void AddStatModified( string stat )
    {
      if (!statsModified.Contains(stat))
        statsModified.Add(stat);
    }

    public void RemoveStatModified( string stat )
    {
      if (statsModified.Contains(stat))
        statsModified.Remove(stat);
    }
#endif
  }
}
