using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Combat/Attack Attributes")]
  public class AttackAttributes : StatsGroup
  {
    [Header("Damage Asset")]
    public Damage Damage;

    [Header("Attack Rate")]
    [StatRange(0f, 5f), // 1.5f
      Tooltip("Interval in seconds unit can attack.")] 
    public Stat AttackRate;
  }
}
