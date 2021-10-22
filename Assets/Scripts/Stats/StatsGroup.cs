using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public abstract class StatsGroup : ScriptableObject
  {
#if UNITY_EDITOR
    [SerializeField, HideInInspector] 
    private List<Stat> stats = new List<Stat>();

    [SerializeField, HideInInspector]
    private List<string> statsNames = new List<string>();

    [SerializeField, HideInInspector] private string nameOfStatsFolder;

    private void OnValidate()
    {
      // Need to be called here since the list seems to get cleared everytime in editor
      FindStatObjects();
    }

    private void OnEnable() 
    {
      // Need to be called here when object is created/selected
      FindStatObjects();
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
#endif
  }
}
