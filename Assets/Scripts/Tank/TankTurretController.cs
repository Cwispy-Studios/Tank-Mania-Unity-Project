using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Tank
{
  using Camera;
  using Combat;
  using Poolers;
  using Projectile;
  using Visuals;

  public class TankTurretController : MonoBehaviour
  {
    [Header("Firing")]
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField, Range(1f, 25f)] private float firingForce = 15f;
    [SerializeField, Range(0.5f, 3f)] private float intervalBetweenFiring = 1.5f;
    [SerializeField, Range(0.1f, 0.5f)] private float timeToQueueFiring = 0.2f;
    [SerializeField] private VfxParentDisabler firingVfx = null;

    [Header("Gun Components")]
    [SerializeField] private GameObject gunCannon = null;
    [SerializeField] private Transform fireZone = null;

    [Header("Reticle Component")]
    [SerializeField] private Image ammoFillImage = null;

    [Header("Turret Rotation")]
    [SerializeField, Range(100f, 360f)] private float turretRotationSpeed = 180f;

    [Header("Damage Information")]
    [SerializeField] private DamageInformation damage;

    private ProjectilePooler projectilePooler;
    private VfxPooler vfxPooler;
    private CameraController playerCamera;

    private float fireCountdown = 0f;
    private bool firingIsQueued = false;

    private float targetTurretRotation;

    private void Awake()
    {
      projectilePooler = FindObjectOfType<ProjectilePooler>();
      vfxPooler = FindObjectOfType<VfxPooler>();

      playerCamera = UnityEngine.Camera.main.GetComponent<CameraController>();
      playerCamera.SetTrackingTarget(this);

      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;

      damage.DamageFrom = Team.Player;
      //damage.Initialise();
      fireCountdown = 0f;
    }

    private void Update()
    {
      UpdateFiringCooldown();

      if (transform.rotation.x != targetTurretRotation)
      {
        RotateTurret();
      }

      RotateGun();
    }

    private void RotateTurret()
    {
      Quaternion to = Quaternion.Euler(0f, targetTurretRotation, 0f);

      transform.rotation = Quaternion.RotateTowards(transform.rotation, to, turretRotationSpeed * Time.deltaTime);
    }

    private void RotateGun()
    {
      float targetGunRotation = Quaternion.LookRotation(playerCamera.GetCrosshairPosition() - gunCannon.transform.position).eulerAngles.x;
      float deltaRotation = targetGunRotation - gunCannon.transform.localEulerAngles.x;

      gunCannon.transform.Rotate(deltaRotation, 0f, 0f, Space.Self);
    }

    private void UpdateFiringCooldown()
    {
      if (fireCountdown > 0f)
      {
        fireCountdown -= Time.deltaTime;

        ammoFillImage.fillAmount = 1f - (fireCountdown / intervalBetweenFiring);

        if (firingIsQueued && fireCountdown <= 0f) FireProjectile();
      }

      else fireCountdown = 0f;
    }

    private void FireProjectile()
    {
      if (fireCountdown <= 0f)
      {
        Projectile projectile = projectilePooler.EnablePooledObject(projectilePrefab, fireZone.position, gunCannon.transform.rotation);
        projectile.PhysicsController.AddForce(gunCannon.transform.forward * firingForce, ForceMode.VelocityChange);
        projectile.SetDamage(damage);

        vfxPooler.EnablePooledObject(firingVfx, fireZone.position, gunCannon.transform.rotation);

        ammoFillImage.fillAmount = 0f;

        fireCountdown += intervalBetweenFiring;

        firingIsQueued = false;
      }

      else if (fireCountdown <= timeToQueueFiring)
      {
        firingIsQueued = true;
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
