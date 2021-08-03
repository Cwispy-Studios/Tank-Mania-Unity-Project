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
    [SerializeField] private Transform gunCannon = null;
    [SerializeField] private Transform fireZone = null;

    [Header("Reticle Component")]
    [SerializeField] private Image ammoFillImage = null;

    [Header("Turret and Gun Rotation")]
    [SerializeField, Range(80f, 360f)] private float turretRotationSpeed = 180f;
    [SerializeField, Range(40f, 300f)] private float gunRotationSpeed = 100f;

    [Header("Damage Information")]
    [SerializeField] private DamageInformation damage;

    private ProjectilePooler projectilePooler;
    private VfxPooler vfxPooler;
    private CameraController playerCamera;

    private float fireCountdown = 0f;
    private bool firingIsQueued = false;

    private float targetTurretRotation;
    private float targetGunRotation;

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
      UpdateFiringCooldownAndFireIfReady();

      if (transform.rotation.x != targetTurretRotation) RotateTurret();

      RotateGun();
    }

    private void RotateTurret()
    {
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

    private void UpdateFiringCooldownAndFireIfReady()
    {
      // Not yet ready to fire
      if (fireCountdown > 0f)
      {
        fireCountdown -= Time.deltaTime;
        if (fireCountdown < 0f) fireCountdown = 0f;

        ammoFillImage.fillAmount = 1f - (fireCountdown / intervalBetweenFiring);
      }

      // Check if ready to fire and player has fired 
      if (firingIsQueued && fireCountdown <= 0f) FireProjectile();
    }

    private void FireProjectile()
    {
      Projectile projectile = projectilePooler.EnablePooledObject(projectilePrefab, fireZone.position, gunCannon.rotation);
      projectile.PhysicsController.AddForce(gunCannon.forward * firingForce, ForceMode.VelocityChange);
      projectile.SetDamage(damage);

      vfxPooler.EnablePooledObject(firingVfx, fireZone.position, gunCannon.rotation);

      ammoFillImage.fillAmount = 0f;

      fireCountdown += intervalBetweenFiring;

      firingIsQueued = false;
    }

    public void ReceiveSignedRotationFromCameraMovement( float horizontalRotation )
    {
      targetTurretRotation = horizontalRotation;
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnMainFire()
    {
      // Queue firing which will be executed in update
      if (fireCountdown <= timeToQueueFiring) firingIsQueued = true;
    }
  }
}
