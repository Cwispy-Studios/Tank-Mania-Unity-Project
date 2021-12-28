using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class TurretController : MonoBehaviour
  {
    [Header("Turret Components")]
    [SerializeField] private Transform mount;
    [SerializeField] private Transform gun;

    [Header("Turret and Gun Rotation")]
    [SerializeField] private TurretRotation turretRotation;
    [SerializeField] private TurretRotationLimits rotationLimits;

    [Header("Rotation Type")]
    [SerializeField] private bool useLocalAngles = true;

    private Transform fireZone;

    private Vector3 mountEulerAngles;
    private Vector3 gunEulerAngles;

    public Transform Mount => mount;
    public Transform Gun => gun;
    public Transform FireZone => fireZone;
    public TurretRotationLimits RotationLimits => rotationLimits;

    public float MountRotationValue => mountEulerAngles.y;
    public float GunRotationValue => gunEulerAngles.x;

    private void Awake()
    {
      fireZone = gun.GetComponent<GunController>().FireZone;
    }

    private void OnEnable()
    {
      mountEulerAngles = useLocalAngles ? mount.localEulerAngles : mount.eulerAngles;
      gunEulerAngles = useLocalAngles ? gun.localEulerAngles : gun.eulerAngles;
    }

    public void RotateMountToValue( float cameraHorizontalRotation )
    {
      mountEulerAngles.x = useLocalAngles ? mount.localEulerAngles.x : mount.eulerAngles.x;
      mountEulerAngles.z = useLocalAngles ? mount.localEulerAngles.z : mount.eulerAngles.z;
      mountEulerAngles.y = Mathf.MoveTowardsAngle(mountEulerAngles.y, cameraHorizontalRotation, turretRotation.MountRotationSpeed.Value * Time.deltaTime);

      if (useLocalAngles) mount.localEulerAngles = mountEulerAngles;
      else mount.eulerAngles = mountEulerAngles;
    }

    public float RotateMountByValue( float value )
    {
      float maxDeltaRotation = turretRotation.MountRotationSpeed.Value * Time.deltaTime;
      float deltaRotation = Mathf.Clamp(value, -maxDeltaRotation, maxDeltaRotation);

      mountEulerAngles.x = useLocalAngles ? mount.localEulerAngles.x : mount.eulerAngles.x;
      mountEulerAngles.z = useLocalAngles ? mount.localEulerAngles.z : mount.eulerAngles.z;
      mountEulerAngles.y += deltaRotation;

      if (rotationLimits.HasYLimits) 
        mountEulerAngles.y = Mathf.Clamp(mountEulerAngles.y, rotationLimits.MinYRot, rotationLimits.MaxYRot);

      if (useLocalAngles) mount.localEulerAngles = mountEulerAngles;
      else mount.eulerAngles = mountEulerAngles;

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

      mount.localRotation = Quaternion.identity;
      gun.localRotation = Quaternion.identity;

      gameObject.SetActive(true);
    }
  }
}
