using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Player/Player Movement")]
  public class PlayerMovement : StatsGroup
  {
    [Header("Acceleration")]
    [StatRange(2f, 5f)] public FloatStat MaxVelocity = new FloatStat(4.5f);
    [StatRange(5f, 25f)] public FloatStat AccelerationForce = new FloatStat(15f);

    [Header("Steering")]
    [StatRange(1f, 5f)] public FloatStat MaxTorque = new FloatStat(1.2f);
    [StatRange(0.5f, 5f)] public FloatStat SteerForce = new FloatStat(1f);
    [StatRange(1f, 5f)] public FloatStat SteerNullifierForce = new FloatStat(3.5f);
  }
}
