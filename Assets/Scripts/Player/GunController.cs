using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  using Combat;
  using Poolers;
  using Stats;
  using Upgrades;

  public class GunController : MonoBehaviour
  {
    [Header("Firing Information")]
    [SerializeField] private FiringInformation firingInformation;
    [SerializeField] private Transform fireZone = null;

    [Header("Damage Information")]
    [SerializeField] private Damage baseDamage;

    [Header("Upgrade Information")]
    [SerializeField] private UpgradedUpgrades upgradedUpgrades;

    [Header("Attributes")]
    [SerializeField] private FloatVariable fireCountdown;
    [SerializeField] private FloatVariable reloadCountdown;
    [SerializeField] private IntVariable currentAmmo;

    private ProjectilePooler projectilePooler;
    private VfxPooler vfxPooler;

    private bool firingIsQueued = false;
    private int previousAmmoCount;

    private void Awake()
    {
      projectilePooler = FindObjectOfType<ProjectilePooler>();
      vfxPooler = FindObjectOfType<VfxPooler>();

      currentAmmo.Value = firingInformation.AmmoCount.Value;
      fireCountdown.Value = 0f;
      reloadCountdown.Value = firingInformation.ReloadSpeed.Value;

      baseDamage.DamageFrom = Team.Player;

      previousAmmoCount = firingInformation.AmmoCount.Value;

      firingInformation.AmmoCount.OnStatUpgrade += OnAmmoChange;
    }

    private void Update()
    {
      HandleFiring();
    }

    private void HandleFiring()
    {
      UpdateFiringCooldown();
      UpdateReloadCooldown();

      // Check if ready to fire and player has fired
      if (firingIsQueued && fireCountdown.Value <= 0f && currentAmmo.Value > 0) 
        FireProjectile();
    }

    private void UpdateFiringCooldown()
    {
      // Not yet ready to fire
      if (fireCountdown.Value > 0f)
      {
        fireCountdown.Value -= Time.deltaTime;
        if (fireCountdown.Value < 0f) fireCountdown.Value = 0f;

        //ammoFillImage.fillAmount = 1f - (fireCountdown.Value / firingInformation.FireRate.Value);
      }
    }

    private void UpdateReloadCooldown()
    {
      if (currentAmmo.Value == firingInformation.AmmoCount.Value) return;

      // Check if reload countdown is still ticking down
      if (reloadCountdown.Value > 0f)
        reloadCountdown.Value -= Time.deltaTime;

      // Check if reload countdown is completed
      if (reloadCountdown.Value <= 0f)
      {
        ++currentAmmo.Value;

        // Ammo count at max, set to max time
        if (currentAmmo.Value == firingInformation.AmmoCount.Value)
          reloadCountdown.Value = firingInformation.ReloadSpeed.Value;

        // Compensate for overflow for the next ammunition to be loaded in
        else
          reloadCountdown.Value += firingInformation.ReloadSpeed.Value;
      }
    }

    private void FireProjectile()
    {
      Projectile projectile = projectilePooler.EnablePooledObject(firingInformation.ProjectilePrefab, fireZone.position, transform.rotation);
      projectile.PhysicsController.AddForce(transform.forward * firingInformation.FiringForce.Value, ForceMode.VelocityChange);
      projectile.SetDamage(baseDamage);
      vfxPooler.EnablePooledObject(firingInformation.FiringVfx, fireZone.position, transform.rotation);

      --currentAmmo.Value;

      fireCountdown.Value += firingInformation.FireRate.Value;

      firingIsQueued = false;
    }

    private void OnAmmoChange()
    {
      // Ammo added
      if (firingInformation.AmmoCount.Value > previousAmmoCount)
      {
        int diff = firingInformation.AmmoCount.Value - previousAmmoCount;
        currentAmmo.Value += diff;
      }

      // Ammo removed
      else
      {
        currentAmmo.Value = Mathf.Clamp(currentAmmo.Value, 0, firingInformation.AmmoCount.Value);
      }

      previousAmmoCount = firingInformation.AmmoCount.Value;
    }

    ///////////////////////////
    // Input Actions callbacks

    public void OnMainFire()
    {
      // Queue firing which will be executed in update
      if (fireCountdown.Value <= firingInformation.TimeToQueueFiring.Value) 
        firingIsQueued = true;
    }
  }
}
