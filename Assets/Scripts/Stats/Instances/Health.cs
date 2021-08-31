using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Health")]
  public class Health : StatsGroup
  {
    public FloatStat MaxHealth = new FloatStat(10f);
    public FloatStat HealthRegeneration = new FloatStat(0.5f);
  }
}
