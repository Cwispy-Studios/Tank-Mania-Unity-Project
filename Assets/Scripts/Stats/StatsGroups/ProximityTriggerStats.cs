using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Proximity Trigger Stats")]
  public class ProximityTriggerStats : StatsGroup
  {
    [StatRange(1, 10)]
    public Stat NumberToTrigger;
    [StatRange(0f, 20f)]
    public Stat TriggerRadius;
    [StatRange(0f, 5f), Tooltip("Time in seconds before the trigger sends its message after being triggered.")]
    public Stat TriggerDelay;
  }
}
