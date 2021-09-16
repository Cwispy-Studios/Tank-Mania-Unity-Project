//using System.Collections.Generic;
//using System.Reflection;

//using UnityEngine;

//namespace CwispyStudios.TankMania.Stats
//{
//  using Upgrades;

//  [CreateAssetMenu(menuName = "Stats/Stat Categoy")]
//  public class StatCategory : ScriptableObject
//  {
//#if UNITY_EDITOR
//    // Editor only, stores the objects to grab the VariableStats from
//    [SerializeField] private List<StatsGroup> statsGroupObjects = new List<StatsGroup>();
//    // Editor only, stores the index of the stat to save
//#pragma warning disable 0414
//    [SerializeField] private int variableStatIndex = 0;
//#pragma warning restore 0414
//#endif

//    // The list of StatModifiers that will affect every stats in this category.
//    [SerializeField] private List<StatModifier> statModifiers = new List<StatModifier>();
//    // The name of the variable that will be subscribed to the above statModifiers
//    [SerializeField] private string variableName;

//    private void OnEnable()
//    {
//      foreach (StatsGroup statsGroup in statsGroupObjects)
//        SubscribeStat(statsGroup, true);
//    }

//    private void OnDisable()
//    {
//      foreach (StatsGroup statsGroup in statsGroupObjects)
//        SubscribeStat(statsGroup, false);
//    }

//    private bool SubscribeStat( object statsGroup, bool subscribe )
//    {
//      // Gets the list of all fields in the object.
//      FieldInfo[] fields = statsGroup.GetType().GetFields();

//      foreach (FieldInfo field in fields)
//      {
//        if (field.Name == variableName)
//        {
//          VariableStat stat = field.GetValue(statsGroup) as VariableStat;

//          //if (subscribe) stat.SubscribeToStatModifiers(statModifiers);
//          //else stat.UnsubscribeFromStatModifiers(statModifiers);

//          return true;
//        }

//        else if (field.FieldType.IsClass)
//          if (SubscribeStat(field.GetValue(statsGroup), subscribe)) return true;
//      }

//      return false;
//    }
//  }
//}
