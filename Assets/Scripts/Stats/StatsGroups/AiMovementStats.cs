using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Enemies/AiMovement")]
  public class AiMovementStats : StatsGroup
  {
    [Header("Velocity")] [StatRange(2f, 50f)]
    public Stat MaxVelocity; // 4.5f

    [Header("Acceleration")]
    public bool UsesAcceleration;
    [StatRange(1f, 25f)] 
    public Stat AccelerationForce; // 15f

    [Header("Steering")] [StatRange(1f, 5f)]
    public bool RotatesWithForce;
    public Stat TurningSpeed;
  }
}