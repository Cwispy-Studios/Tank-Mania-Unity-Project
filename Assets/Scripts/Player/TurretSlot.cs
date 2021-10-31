using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public class TurretSlot : MonoBehaviour
  {
    [SerializeField] private TurretRotationLimits turretRotationLimits;
    [SerializeField] private Vector3 turretRotation;

    public TurretRotationLimits RotationLimits => turretRotationLimits;

    //private Turret occupiedTurret;
    //public Turret OccupiedTurret => occupiedTurret;

    //private bool isUnlocked = false;
    //public bool IsUnlocked => isUnlocked;

    //public bool IsOccupied => occupiedTurret != null;

    //public void SetTurretOnSlot( Turret turret )
    //{
    //  occupiedTurret = turret;
    //}
  }
}
