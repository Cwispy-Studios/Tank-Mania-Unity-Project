using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Player/Player Movement")]
  public class PlayerMovement : StatsGroup
  {
    [Header("Acceleration")]
    [StatRange(2f, 5f)] public Stat MaxVelocity; // 4.5f
    [StatRange(5f, 25f)] public Stat AccelerationForce; // 15f

    [Header("Steering")]
    [StatRange(1f, 5f)] public Stat MaxTorque; // 1.2f
    [StatRange(0.5f, 5f)] public Stat SteerForce; // 1f
    [StatRange(1f, 5f)] public Stat SteerNullifierForce; // 3.5f
  }
}
