using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Stats Category", order = 10)]
  public class StatsCategory : ScriptableObject
  {
#if UNITY_EDITOR
    [Header("Assets Finder")]
    [SerializeField] private StatsCategoryStatNamesInPath[] statNamesInPaths;
    [SerializeField] private bool autoRetrieve;
#endif

    [Header("Stats in category")]
    [SerializeField] private List<Stat> statsInCategory;
    public IReadOnlyCollection<Stat> StatsInCategory => statsInCategory;
  }
}
