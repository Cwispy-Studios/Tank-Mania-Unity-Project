using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Stats Category", order = 100)]
  public class StatsCategory : ScriptableObject
  {
    [SerializeField] private List<Stat> statsInCategory;
    public IReadOnlyCollection<Stat> StatsInCategory => statsInCategory;
  }
}
