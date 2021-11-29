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
    [SerializeField] private TurretRotationLimits rotationLimits;

    [Header("Rotation Type")]
    [SerializeField] private bool useLocalAngles = true;

    private Vector3 turretEulerAngles;
    private Vector3 gunEulerAngles;

    public Transform Turret => turret;
    public Transform Gun => gun;
    public TurretRotationLimits RotationLimits => rotationLimits;

    public float TurretRotationValue => turretEulerAngles.y;
    public float GunRotationValue => gunEulerAngles.x;

    private void OnEnable()
    {
      turretEulerAngles = useLocalAngles ? turret.localEulerAngles : turret.eulerAngles;
      gunEulerAngles = useLocalAngles ? gun.localEulerAngles : gun.eulerAngles;
    }

    public void RotateTurretToValue( float cameraHorizontalRotation )
    {
      turretEulerAngles.x = useLocalAngles ? turret.localEulerAngles.x : turret.eulerAngles.x;
      turretEulerAngles.z = useLocalAngles ? turret.localEulerAngles.z : turret.eulerAngles.z;
      turretEulerAngles.y = Mathf.MoveTowardsAngle(turretEulerAngles.y, cameraHorizontalRotation, turretRotation.TurretRotationSpeed.Value * Time.deltaTime);

      if (useLocalAngles) turret.localEulerAngles = turretEulerAngles;
      else turret.eulerAngles = turretEulerAngles;
    }

    public float RotateTurretByValue( float value )
    {
      float maxDeltaRotation = turretRotation.TurretRotationSpeed.Value * Time.deltaTime;
      float deltaRotation = Mathf.Clamp(value, -maxDeltaRotation, maxDeltaRotation);

      turretEulerAngles.x = useLocalAngles ? turret.localEulerAngles.x : turret.eulerAngles.x;
      turretEulerAngles.z = useLocalAngles ? turret.localEulerAngles.z : turret.eulerAngles.z;
      turretEulerAngles.y += deltaRotation;

      if (rotationLimits.HasYLimits) 
        turretEulerAngles.y = Mathf.Clamp(turretEulerAngles.y, rotationLimits.MinYRot, rotationLimits.MaxYRot);

      if (useLocalAngles) turret.localEulerAngles = turretEulerAngles;
      else turret.eulerAngles = turretEulerAngles;

      return deltaRotation;
    }

    public void RotateGunToValue( float cameraVerticalRotation )
    {
      gunEulerAngles.y = useLocalAngles ? gun.localEulerAngles.y : gun.eulerAngles.y;
      gunEulerAngles.z = useLocalAngles ? gun.localEulerAngles.z : gun.eulerAngles.z;
      gunEulerAngles.x = Mathf.MoveTowardsAngle(gunEulerAngles.x, cameraVerticalRotation, turretRotation.GunRotationSpeed.Value * Time.deltaTime);

      if (useLocalAngles) gun.localEulerAngles = gunEulerAngles;
      else gun.eulerAngles = gunEulerAngles;
    }

    public float RotateGunByValue( float value )
    {
      float maxDeltaRotation = turretRotation.GunRotationSpeed.Value * Time.deltaTime;
      float deltaRotation = Mathf.Clamp(value, -maxDeltaRotation, maxDeltaRotation);

      gunEulerAngles.y = useLocalAngles ? gun.localEulerAngles.y : gun.eulerAngles.y;
      gunEulerAngles.z = useLocalAngles ? gun.localEulerAngles.z : gun.eulerAngles.z;
      gunEulerAngles.x += deltaRotation;

      if (rotationLimits.HasXLimits)
        gunEulerAngles.x = Mathf.Clamp(gunEulerAngles.x, rotationLimits.MinXRot, rotationLimits.MaxXRot);

      if (useLocalAngles) gun.localEulerAngles = gunEulerAngles;
      else gun.eulerAngles = gunEulerAngles;

      return deltaRotation;
    }

    public void AssignToSlot( TurretSlot slot )
    {
      transform.parent = slot.transform.parent;
      transform.position = slot.transform.position;
      transform.rotation = slot.transform.rotation;

      rotationLimits = slot.RotationLimits;

      turret.localRotation = Quaternion.identity;
      gun.localRotation = Quaternion.identity;

      gameObject.SetActive(true);
    }
  }
}
