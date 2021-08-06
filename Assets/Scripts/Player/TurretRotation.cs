using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/Turret Rotation")]
  public class TurretRotation : ScriptableObject
  {
    [Range(80f, 360f)] public float TurretRotationSpeed = 120f;
    [Range(40f, 300f)] public float GunRotationSpeed = 60f;
  }
}
