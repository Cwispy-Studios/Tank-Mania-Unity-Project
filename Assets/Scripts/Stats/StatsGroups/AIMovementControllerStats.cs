using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Enemies/AiMovement")]
  public class AIMovementControllerStats : StatsGroup
  {
    [Header("Velocity")] [StatRange(0f, 50f)]
    public Stat MaxVelocity; // 4.5f

    [Header("Acceleration")]
    public bool UsesAcceleration;
    [StatRange(0f, 5f)] 
    public Stat AccelerationForce; // 15f

    [Header("Steering")] [StatRange(1f, 5f)]
    public bool RotatesWithForce;
    public Stat TurningSpeed;
  }
}