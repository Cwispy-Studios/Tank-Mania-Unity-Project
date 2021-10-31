using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/Gun Rotation Limits")]
  public class TurretRotationLimits : ScriptableObject
  {
    [Header("X-Axis")]
    public bool HasXLimits = true;
    [Range(-180f, 180f)] public float MaxXRot;
    [Range(-180f, 180f)] public float MinXRot;

    [Header("Y-Axis")]
    public bool HasYLimits = true;
    [Range(180f, 180f)] public float MinYRot;
    [Range(180f, 180f)] public float MaxYRot;
  }
}
