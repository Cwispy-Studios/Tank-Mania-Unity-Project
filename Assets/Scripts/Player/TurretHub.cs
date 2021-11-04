using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class TurretHub : MonoBehaviour
  {
    [Header("Turret Components")]
    [SerializeField] private Transform turret;
    [SerializeField] private Transform gun;

    [Header("Turret and Gun Rotation")]
    [SerializeField] private TurretRotation turretRotation;
    [SerializeField] private TurretRotationLimits turretRotationLimits;

    public Transform Turret => turret;
    public Transform Gun => gun;
    public TurretRotationLimits TurretRotationLimits => turretRotationLimits;

    public void RotateTurretToValue( float cameraHorizontalRotation )
    {
      if (turret.transform.rotation.x == cameraHorizontalRotation) return;

      Quaternion to = Quaternion.Euler(0f, cameraHorizontalRotation, 0f);
      turret.transform.rotation = 
        Quaternion.RotateTowards(turret.transform.rotation, to, turretRotation.TurretRotationSpeed.Value * Time.deltaTime);
    }

    public void RotateTurretByValue( float value )
    {
      turret.Rotate(0f, value, 0f, Space.Self);
    }

    public void RotateGunToValue( float cameraVerticalRotation )
    {
      if (gun.localRotation.eulerAngles.x != cameraVerticalRotation)
      {
        Quaternion to = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
        gun.localRotation =
          Quaternion.RotateTowards(gun.localRotation, to, turretRotation.GunRotationSpeed.Value * Time.deltaTime);
      }
    }

    public void RotateGunByValue( float value )
    {
      gun.Rotate(value, 0f, 0f, Space.Self);
    }

    public void AssignToSlot( TurretSlot slot )
    {
      transform.parent = slot.transform.parent;
      transform.position = slot.transform.position;
      transform.rotation = slot.transform.rotation;

      turretRotationLimits = slot.RotationLimits;

      gameObject.SetActive(true);
    }
  }
}