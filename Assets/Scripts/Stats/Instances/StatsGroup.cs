using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  public abstract class StatsGroup : ScriptableObject
  {
    [SerializeField, HideInInspector] 
    private List<VariableStat> stats = new List<VariableStat>();

#if UNITY_EDITOR
    private List<string> statsName = new List<string>();
#endif

#if !UNITY_EDITOR
    private void Awake()
    {
      // Called for build version
      FindStatObjects();
    }
#endif

    private void OnValidate()
    {
      Debug.Log("Validate");
      // Need to be called here since the list seems to get cleared everytime in editor
      FindStatObjects();

      GetStatModifiersFromIndex();

      // Does not need to be called in build since the values get serialized in editor already
      SetDefaultUpgradedStatValues();
    }

    private void GetStatModifiersFromIndex()
    {
      foreach (VariableStat stat in stats)
      {
        foreach (UpgradeSubscription upgradeSubscription in stat.UpgradeSubscriptions)
        {
          upgradeSubscription.AssignStatModifierFromInspectorSelection();
          upgradeSubscription.ValidateUpgrade();
        }
      }
    }

    private void ValidateUpgradesOfStatModifiers()
    {
      foreach (VariableStat stat in stats)
      {
        foreach (UpgradeSubscription upgradeSubscription in stat.UpgradeSubscriptions)
        {
          upgradeSubscription.ValidateUpgrade();
        }
      }
    }

    private void OnEnable() 
    {
      Debug.Log("Enable");

#if UNITY_EDITOR
      ValidateUpgradesOfStatModifiers();
#endif

      SubscribeStats();
    }

    private void OnDisable()
    {
      UnsubscribeStats();
    }

    private void FindStatObjects()
    {
      stats.Clear();
      statsName.Clear();
      GetStatVariables(this);
    }

    private void GetStatVariables( object statsGroup )
    {
      // Gets the list of all fields in the object.
      FieldInfo[] fields = statsGroup.GetType().GetFields();

      foreach (FieldInfo field in fields)
      {
        // Find the fields that are VariableStats
        if (field.FieldType == typeof(FloatStat) || field.FieldType == typeof(IntStat))
        {
          stats.Add(field.GetValue(statsGroup) as VariableStat);
          statsName.Add(field.Name);
        }

        else if (field.FieldType.IsClass)
          GetStatVariables(field.GetValue(statsGroup));
      }
    }

    private void SetDefaultUpgradedStatValues()
    {
      foreach (VariableStat stat in stats)
        stat?.SetDefaultUpgradedValue();
    }

    private void SubscribeStats()
    {
      foreach (VariableStat stat in stats)
        stat.SubscribeToUpgradeInstances();
    }
    private void UnsubscribeStats()
    {
      foreach (VariableStat stat in stats)
        stat.UnsubscribeFromUpgradeInstances();
    }
  }
}
