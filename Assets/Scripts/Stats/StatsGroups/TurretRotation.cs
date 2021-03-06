using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Player/Turret Rotation")]
  public class TurretRotation : StatsGroup
  {
    [StatRange(80f, 360f)] public Stat TurretRotationSpeed; // 120f
    [StatRange(40f, 300f)] public Stat GunRotationSpeed; // 60f
  }
}
