using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Health")]
  public class Health : StatsGroup
  {
    public Stat MaxHealth = new Stat(10f);
    public Stat HealthRegeneration = new Stat(0.5f);
  }
}
