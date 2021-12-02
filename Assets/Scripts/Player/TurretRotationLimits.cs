using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/Gun Rotation Limits")]
  public class TurretRotationLimits : ScriptableObject
  {
    [Header("X-Axis")]
    public bool HasXLimits = true;
    public float MaxXRot;
    public float MinXRot;

    [Header("Y-Axis")]
    public bool HasYLimits = true;
    public float MinYRot;
    public float MaxYRot;

    //public float ConvertAngleToWithinXRotationLimits( float angle )
    //{

    //}

    //private float ConvertAngleToWithinRotationLimits( float angle )
    //{

    //}
  }
}
