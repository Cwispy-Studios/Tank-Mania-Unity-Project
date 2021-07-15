using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Tank
{
  using Projectile;
  using Camera;

  public class TankTurretController : MonoBehaviour
  {
    [Header("Firing")]
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField, Range(1f, 25f)] private float firingForce = 15f;
    [SerializeField, Range(0.5f, 3f)] private float intervalBetweenFiring = 1.5f;

    [Header("Gun Components")]
    [SerializeField] private GameObject gunCannon = null;
    [SerializeField] private Transform fireZone = null;

    [Header("Reticle Component")]
    [SerializeField] private Image ammoFillImage = null;

    [Header("Turret Rotation")]
    [SerializeField, Range(100f, 360f)] private float turretRotationSpeed = 180f;

    private ProjectilePooler projectilePooler;
    private CameraController playerCamera;

    private float fireCountdown = 0f;

    private float targetTurretRotation;

    private void Awake()
    {
      projectilePooler = FindObjectOfType<ProjectilePooler>();

      playerCamera = UnityEngine.Camera.main.GetComponent<CameraController>();
      playerCamera.SetTrackingTarget(this);

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      fireCountdown = 0f;
    }

    private void Update()
    {
      UpdateCooldown();

      if (transform.rotation.x != targetTurretRotation)
      {
        RotateTurret();
      }

      RotateGun();
    }

    private void RotateTurret()
    {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, targetTurretRotation, 0f), turretRotationSpeed * Time.deltaTime);
    }

    private void RotateGun()
    {
      float targetGunRotation = Quaternion.LookRotation(playerCamera.GetCrosshairPosition() - gunCannon.transform.position).eulerAngles.x;
      float deltaRotation = targetGunRotation - gunCannon.transform.localEulerAngles.x;

      gunCannon.transform.Rotate(deltaRotation, 0f, 0f, Space.Self);
    }

    private void UpdateCooldown()
    {
      if (fireCountdown > 0f)
      {
        fireCountdown -= Time.deltaTime;

        ammoFillImage.fillAmount = 1f - (fireCountdown / intervalBetweenFiring);
      }

      else fireCountdown = 0f;
    }

    private void FireProjectile()
    {
      if (fireCountdown <= 0f)
      {
        Projectile projectile = projectilePooler.EnableProjectile(projectilePrefab, fireZone.position, gunCannon.transform.rotation);
        projectile.PhysicsController.AddForce(gunCannon.transform.forward * firingForce, ForceMode.VelocityChange);

        ammoFillImage.fillAmount = 0f;

        fireCountdown += intervalBetweenFiring;
      }
    }

    public void ReceiveSignedRotationFromCameraMovement( float horizontalRotation )
    {
      targetTurretRotation = horizontalRotation;
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnMainFire()
    {
      FireProjectile();
    }
  }
}
