using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Proximity Trigger Stats")]
  public class ProximityTriggerStats : TriggerStats
  {
    [StatRange(1, 10)]
    public Stat NumberToTrigger;
    [StatRange(0f, 20f)]
    public Stat TriggerRadius;
  }
}
