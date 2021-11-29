using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public class TriggerStats : StatsGroup
  {
    [StatRange(0f, 5f), Tooltip("Time in seconds before the trigger sends its message after being triggered.")]
    public Stat TriggerDelay;
  }
}
