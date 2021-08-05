using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Camera;

  public class TankTurretController : MonoBehaviour
  {
    [Header("Gun Components")]
    [SerializeField] private Transform gunCannon = null;

    [Header("Turret and Gun Rotation")]
    [SerializeField, Range(80f, 360f)] private float turretRotationSpeed = 180f;
    [SerializeField, Range(40f, 300f)] private float gunRotationSpeed = 100f;

    private CameraController playerCamera;

    private float targetTurretRotation;
    private float targetGunRotation;

    private void Awake()
    {
      playerCamera = UnityEngine.Camera.main.GetComponent<CameraController>();
      playerCamera.SetTrackingTarget(this);

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
      if (transform.rotation.x == targetTurretRotation) return;

      Quaternion to = Quaternion.Euler(0f, targetTurretRotation, 0f);
      transform.rotation = Quaternion.RotateTowards(transform.rotation, to, turretRotationSpeed * Time.deltaTime);
    }

    private void RotateGun()
    {
      Vector3 crossHairGunCannonDiff = playerCamera.GetCrosshairPosition() - gunCannon.position;
      targetGunRotation = Quaternion.LookRotation(crossHairGunCannonDiff).eulerAngles.x;

      if (gunCannon.localRotation.x != targetGunRotation)
      {
        Quaternion to = Quaternion.Euler(targetGunRotation, 0f, 0f);

        gunCannon.localRotation = Quaternion.RotateTowards(gunCannon.localRotation, to, gunRotationSpeed * Time.deltaTime);
      }
    }

    public void ReceiveSignedRotationFromCameraMovement( float horizontalRotation )
    {
      targetTurretRotation = horizontalRotation;
    }
  }
}
