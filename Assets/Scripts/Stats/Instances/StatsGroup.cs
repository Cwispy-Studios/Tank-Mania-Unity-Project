using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public abstract class StatsGroup : ScriptableObject
  {
    [SerializeField, HideInInspector] 
    private List<VariableStat> stats = new List<VariableStat>();

#if UNITY_EDITOR
    private List<VariableStat> oldStats = new List<VariableStat>();
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
    }

    private void OnEnable() 
    {
      SubscribeStats();
    }

    private void OnDisable()
    {
      UnsubscribeStats();
    }

    private void FindStatObjects()
    {
      stats.Clear();
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
          stats.Add(field.GetValue(statsGroup) as VariableStat);

        else if (field.FieldType.IsClass)
          GetStatVariables(field.GetValue(statsGroup));
      }
    }

    //private void Test()
    //{
    //  foreach (VariableStat stat in stats)
    //    stat?.StatModifiers[0].;
    //}

    private void SetDefaultUpgradedStatValues()
    {
      foreach (VariableStat stat in stats)
        stat?.SetDefaultUpgradedValue();
    }

    private void SubscribeStats()
    {
      foreach (VariableStat stat in stats)
        stat.SubscribeToStatModifiers();
    }
    private void UnsubscribeStats()
    {
      foreach (VariableStat stat in stats)
        stat.UnsubscribeFromStatModifiers();
    }
  }
}
