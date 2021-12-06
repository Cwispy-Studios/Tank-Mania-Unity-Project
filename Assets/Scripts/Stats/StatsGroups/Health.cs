using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Combat/Health")]
  public class Health : StatsGroup
  {
    public Stat MaxHealth;
    public Stat HealthRegeneration;
  }
}
