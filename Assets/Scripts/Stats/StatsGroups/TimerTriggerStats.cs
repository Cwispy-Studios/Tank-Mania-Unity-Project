using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Triggers/Time Trigger Stats")]
  public class TimerTriggerStats : TriggerStats
  {
    [StatRange(0f, 3000f), Tooltip("Time in seconds before trigger")]
    public Stat TimeToTrigger;
  }
}
