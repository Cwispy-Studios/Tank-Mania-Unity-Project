using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  public abstract class StatsGroup : ScriptableObject
  {
    [SerializeField, HideInInspector] 
    private List<Stat> stats = new List<Stat>();

#if UNITY_EDITOR
    [SerializeField, HideInInspector]
    private List<string> statsNames = new List<string>();

    [SerializeField, HideInInspector] private string nameOfStatsFolder;
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
      // Need to be called here since the list seems to get cleared everytime in editor
      FindStatObjects();

      // Does not need to be called in build since the values get serialized in editor already
      SetDefaultUpgradedStatValues();

      //InformStatModifiers();
    }

    private void OnEnable() 
    {
#if UNITY_EDITOR
      // Need to be called here when object is created
      FindStatObjects();
      //InformStatModifiers();
#endif

      //SubscribeStats();
    }

    private void OnDisable()
    {
      UnsubscribeStats();
    }

    private void FindStatObjects()
    {
      stats.Clear();
      statsNames.Clear();
      GetStatVariables(this);
    }

    private void GetStatVariables( object statsGroup )
    {
      // Gets the list of all fields in the object.
      FieldInfo[] fields = statsGroup.GetType().GetFields();

      foreach (FieldInfo field in fields)
      {
        // Find the fields that are Stats
        if (field.FieldType == typeof(Stat))
        {
          stats.Add(field.GetValue(statsGroup) as Stat);
          statsNames.Add(field.Name);
        }

        else if (field.DeclaringType == typeof(StatsGroup))
          GetStatVariables(field.GetValue(statsGroup));
      }
    }

    private void InformStatModifiers()
    {
      for (int i = 0; i < stats.Count; ++i)
      {
        foreach (StatModifier statModifier in stats[i].StatModifiers) 
        {
          statModifier?.AddStat(this, statsNames[i], stats[i]);
        }
      }
    }

    private void SetDefaultUpgradedStatValues()
    {
      foreach (Stat stat in stats)
        stat?.SetDefaultUpgradedValue();
    }

    private void SubscribeStats()
    {
      foreach (Stat stat in stats)
        stat.SubscribeToStatModifiers();
    }
    private void UnsubscribeStats()
    {
      foreach (Stat stat in stats)
        stat.UnsubscribeFromStatModifiers();
    }
  }
}
