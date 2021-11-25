using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class TurretSlot : MonoBehaviour
  {
    [SerializeField] private TurretRotationLimits turretRotationLimits;

    public TurretRotationLimits RotationLimits => turretRotationLimits;
  }
}
