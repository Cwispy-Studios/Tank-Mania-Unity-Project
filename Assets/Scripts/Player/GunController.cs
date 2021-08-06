using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  using Combat;
  using Poolers;
  using Projectile;

  public class GunController : MonoBehaviour
  {
    [Header("Reticle Component")]
    [SerializeField] private Image ammoFillImage;

    [Header("Firing Information")]
    [SerializeField] private FiringInformation firingInformation;
    [SerializeField] private Transform fireZone = null;

    [Header("Damage Information")]
    [SerializeField] private Damage baseDamage;

    private ProjectilePooler projectilePooler;
    private VfxPooler vfxPooler;

    private float fireCountdown = 0f;
    private bool firingIsQueued = false;

    private void Awake()
    {
      projectilePooler = FindObjectOfType<ProjectilePooler>();
      vfxPooler = FindObjectOfType<VfxPooler>();

      fireCountdown = 0f;

      baseDamage.DamageFrom = Team.Player;
    }

    private void Update()
    {
      UpdateFiringCooldownAndFireIfReady();
    }

    private void UpdateFiringCooldownAndFireIfReady()
    {
      // Not yet ready to fire
      if (fireCountdown > 0f)
      {
        fireCountdown -= Time.deltaTime;
        if (fireCountdown < 0f) fireCountdown = 0f;

        ammoFillImage.fillAmount = 1f - (fireCountdown / firingInformation.IntervalBetweenFiring);
      }

      // Check if ready to fire and player has fired 
      if (firingIsQueued && fireCountdown <= 0f) FireProjectile();
    }

    private void FireProjectile()
    {
      Projectile projectile = projectilePooler.EnablePooledObject(firingInformation.ProjectilePrefab, fireZone.position, transform.rotation);
      projectile.PhysicsController.AddForce(transform.forward * firingInformation.FiringForce, ForceMode.VelocityChange);
      projectile.SetDamage(baseDamage);

      vfxPooler.EnablePooledObject(firingInformation.FiringVfx, fireZone.position, transform.rotation);

      ammoFillImage.fillAmount = 0f;

      fireCountdown += firingInformation.IntervalBetweenFiring;

      firingIsQueued = false;
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnMainFire()
    {
      // Queue firing which will be executed in update
      if (fireCountdown <= firingInformation.TimeToQueueFiring) firingIsQueued = true;
    }
  }
}
