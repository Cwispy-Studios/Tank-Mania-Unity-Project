using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class Turret : MonoBehaviour
  {
    [Header("Turret Components")]
    [SerializeField] private Transform turret;
    [SerializeField] private Transform gun;

    [Header("Turret and Gun Rotation")]
    [SerializeField] private TurretRotation turretRotation;
    [SerializeField] private TurretRotationLimits turretRotationLimits;

    [Header("Camera Variables")]
    [SerializeField] private FloatVariable cameraHorizontalRotation;

    private CameraController playerCamera;

    private float targetGunRotation;
    private bool isPlayerControlled = true;

    public TurretRotationLimits TurretRotationLimits => turretRotationLimits;

    private void Awake()
    {
      playerCamera = Camera.main.GetComponent<CameraController>();

      // Move somewhere else
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    private void Update()
    {
      if (isPlayerControlled)
      {
        RotateTurret();
        RotateGun();
      }
    }

    private void RotateTurret()
    {
      if (turret.transform.rotation.x == cameraHorizontalRotation.Value) return;

      Quaternion to = Quaternion.Euler(0f, cameraHorizontalRotation.Value, 0f);
      turret.transform.rotation = 
        Quaternion.RotateTowards(turret.transform.rotation, to, turretRotation.TurretRotationSpeed.Value * Time.deltaTime);
    }

    private void RotateGun()
    {
      Vector3 crossHairGunCannonDiff = playerCamera.GetCrosshairPosition() - gun.position;
      targetGunRotation = Quaternion.LookRotation(crossHairGunCannonDiff).eulerAngles.x;
      
      if (gun.localRotation.x != targetGunRotation)
      {
        Quaternion to = Quaternion.Euler(targetGunRotation, 0f, 0f);
        gun.localRotation = 
          Quaternion.RotateTowards(gun.localRotation, to, turretRotation.GunRotationSpeed.Value * Time.deltaTime);
      }
    }

    public void SetTurretRotationLimits( TurretRotationLimits turretRotationLimits )
    {
      this.turretRotationLimits = turretRotationLimits;
      isPlayerControlled = false;
    }
  }
}
