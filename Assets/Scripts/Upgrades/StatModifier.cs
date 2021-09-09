using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [CreateAssetMenu(menuName = "Upgrades/Stat Modifier")]
  public class StatModifier : ScriptableObject
  {
    [SerializeField] private float additiveValue;
    public float AddititiveValue => additiveValue;

    [SerializeField] private float multiplicativeValue;
    public float MultiplicativeValue => multiplicativeValue;

    public Action OnStatUpgrade;

#if UNITY_EDITOR
    // This will display the property in the inspector
    [SerializeField] private List<StatsGroup> statsGroupsModified = new List<StatsGroup>();
    // This will display the exact name of the stat being modified in the inspector
    [SerializeField] private List<string> statNamesModified = new List<string>();
    // This is to store the actual stat being modified to check within this script, not to be used by inspector
    [SerializeField] private List<VariableStat> statsModified = new List<VariableStat>();

    /// <summary>
    /// DO NOT USE THIS FUNCTION IF YOU DON'T KNOW WHAT IT DOES, ONLY FOR EDITOR USE
    /// </summary>
    public void AddStat( StatsGroup statsGroup, string statName, VariableStat stat )
    {
      VerifyStatLists();

      if (!statNamesModified.Contains(statName))
      {
        statsGroupsModified.Add(statsGroup);
        statNamesModified.Add(statName);
        statsModified.Add(stat);
      }

      CheckIfStillModifiesStats();
    }

    public void RefreshStatsList()
    {
      VerifyStatLists();
      CheckIfStillModifiesStats();
    }

    private void VerifyStatLists()
    {
      // In case any of the list variable name gets changed, it will get cleared while the others are still populated
      // This ensures everything is resetted when that happens
      if (statsGroupsModified.Count != statNamesModified.Count || statsGroupsModified.Count != statsModified.Count)
      {
        statsGroupsModified.Clear();
        statNamesModified.Clear();
        statsModified.Clear();
      }
    }

    private void CheckIfStillModifiesStats()
    {
      // Removes any stat that no longer contains this stat modifier
      //for (int i = statsModified.Count - 1; i >= 0; --i)
      //{
      //  if (!statsModified[i].UpgradeSubscriptions.Contains(this))
      //  {
      //    statsGroupsModified.RemoveAt(i);
      //    statNamesModified.RemoveAt(i);
      //    statsModified.RemoveAt(i);
      //  }
      //}
    }
#endif
  }
}
