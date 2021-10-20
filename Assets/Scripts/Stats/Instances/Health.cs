using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Health")]
  public class Health : StatsGroup
  {
    public Stat MaxHealth;
    public Stat HealthRegeneration;
  }
}
