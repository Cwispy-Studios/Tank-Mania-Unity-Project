using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Player/Turret Rotation")]
  public class TurretRotation : StatsGroup
  {
    [StatRange(80f, 360f)] public FloatStat TurretRotationSpeed = new FloatStat(120f);
    [StatRange(40f, 300f)] public FloatStat GunRotationSpeed = new FloatStat(60f);
  }
}
