using UnityEngine;

namespace CwispyStudios.TankMania.Tank
{
  using Projectile;
  using Camera;

  public class TankTurretController : MonoBehaviour
  {
    [Header("Firing")]
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField, Range(1f, 25f)] private float firingForce = 15f;

    [Header("Gun Component")]
    [SerializeField] private GameObject gunCannon = null;
    [SerializeField] private Transform fireZone = null;

    [Header("Turret Rotation")]
    [SerializeField, Range(100f, 360f)] private float turretRotationSpeed = 180f;

    private ProjectilePooler projectilePooler;
    private CameraController playerCamera;
    private float targetTurretRotation;
    private float targetGunRotation;

    private void Awake()
    {
      projectilePooler = FindObjectOfType<ProjectilePooler>();

      playerCamera = UnityEngine.Camera.main.GetComponent<CameraController>();
      playerCamera.SetTrackingTarget(this);

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    private void Update()
    {
      if (transform.rotation.x != targetTurretRotation)
      {
        RotateTurret();
      }

      if (gunCannon.transform.localEulerAngles.x != targetGunRotation)
      {
        RotateGun();
      }
    }

    private void RotateTurret()
    {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, targetTurretRotation, 0f), turretRotationSpeed * Time.deltaTime);
    }

    private void RotateGun()
    {
      float deltaRotation = targetGunRotation - gunCannon.transform.localEulerAngles.x - playerCamera.BaseRotation;

      gunCannon.transform.Rotate(deltaRotation, 0f, 0f, Space.Self);
    }

    public void ReceiveSignedRotation( float horizontalRotation, float verticalRotation )
    {
      targetTurretRotation = horizontalRotation;
      targetGunRotation = verticalRotation;
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnMainFire()
    {
      Projectile projectile = projectilePooler.EnableProjectile(projectilePrefab, fireZone.position, gunCannon.transform.rotation);
      projectile.PhysicsController.AddForce(gunCannon.transform.forward * firingForce, ForceMode.VelocityChange);
    }
  }
}
