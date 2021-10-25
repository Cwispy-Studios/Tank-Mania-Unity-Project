using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class TankTurretController : MonoBehaviour
  {
    [Header("Gun Components")]
    [SerializeField] private Transform gunCannon;

    [Header("Turret and Gun Rotation")]
    [SerializeField] private TurretRotation turretRotation;
    [SerializeField] private GunRotationLimits gunRotationLimits;

    [Header("Camera Variables")]
    [SerializeField] private FloatVariable cameraHorizontalRotation;

    private CameraController playerCamera;

    private float targetGunRotation;

    private void Awake()
    {
      playerCamera = Camera.main.GetComponent<CameraController>();
      playerCamera.SetTrackingTarget(this, gunRotationLimits);

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    private void Update()
    {
      RotateTurret();
      RotateGun();
    }

    private void RotateTurret()
    {
      if (transform.rotation.x == cameraHorizontalRotation.Value) return;

      Quaternion to = Quaternion.Euler(0f, cameraHorizontalRotation.Value, 0f);
      transform.rotation = 
        Quaternion.RotateTowards(transform.rotation, to, turretRotation.TurretRotationSpeed.Value * Time.deltaTime);
    }

    private void RotateGun()
    {
      Vector3 crossHairGunCannonDiff = playerCamera.GetCrosshairPosition() - gunCannon.position;
      targetGunRotation = Quaternion.LookRotation(crossHairGunCannonDiff).eulerAngles.x;

      if (gunCannon.localRotation.x != targetGunRotation)
      {
        Quaternion to = Quaternion.Euler(targetGunRotation, 0f, 0f);
        gunCannon.localRotation = 
          Quaternion.RotateTowards(gunCannon.localRotation, to, turretRotation.GunRotationSpeed.Value * Time.deltaTime);
      }
    }
  }
}
