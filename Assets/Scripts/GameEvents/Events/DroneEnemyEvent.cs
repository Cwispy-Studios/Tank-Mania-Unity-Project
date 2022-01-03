using UnityEngine;

namespace CwispyStudios.TankMania.GameEvents
{
  using Enemy;

  [CreateAssetMenu(fileName = "New Drone Enemy Event", menuName = "Game Events/Drone Enemy Event")]
  public class DroneEnemyEvent : BaseGameEvent<DroneEnemy> { }
}