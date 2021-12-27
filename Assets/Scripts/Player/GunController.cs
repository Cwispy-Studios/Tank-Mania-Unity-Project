using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Combat;
  using Poolers;
  using Stats;

  public class GunController : MonoBehaviour
  {
    [Header("Attack and Firing Attributes")]
    [SerializeField] private ProjectileAttackAttributes attackAttributes;
    [SerializeField] private Transform fireZone = null;

    [Header("Attributes")]
    [SerializeField] private FloatVariable fireCountdown;
    [SerializeField] private FloatVariable reloadCountdown;
    [SerializeField] private IntVariable currentAmmo;

    public Transform FireZone => fireZone;

    private ProjectilePool projectilePool;
    private VfxPool vfxPool;

    private bool CanFire => fireCountdown.Value <= 0f && currentAmmo.Value > 0;

    private bool firingIsQueued = false;
    private int previousAmmoCount;

    private void Awake()
    {
      projectilePool = FindObjectOfType<ProjectilePool>();
      vfxPool = FindObjectOfType<VfxPool>();

      currentAmmo.Value = (int)attackAttributes.AmmoCount.Value;
      fireCountdown.Value = 0f;
      reloadCountdown.Value = attackAttributes.ReloadSpeed.Value;

      previousAmmoCount = (int)attackAttributes.AmmoCount.Value;

      attackAttributes.AmmoCount.OnStatUpgrade += OnAmmoChange;
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
      if (CanFire && firingIsQueued) FireProjectile();
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
      if (currentAmmo.Value == attackAttributes.AmmoCount.Value) return;

      // Check if reload countdown is still ticking down
      if (reloadCountdown.Value > 0f)
        reloadCountdown.Value -= Time.deltaTime;

      // Check if reload countdown is completed
      if (reloadCountdown.Value <= 0f)
      {
        ++currentAmmo.Value;

        // Ammo count at max, set to max time
        if (currentAmmo.Value == attackAttributes.AmmoCount.Value)
          reloadCountdown.Value = attackAttributes.ReloadSpeed.Value;

        // Compensate for overflow for the next ammunition to be loaded in
        else
          reloadCountdown.Value += attackAttributes.ReloadSpeed.Value;
      }
    }

    private void FireProjectile()
    {
      Projectile projectile = projectilePool.EnablePooledObject(attackAttributes.ProjectilePrefab, fireZone.position, transform.rotation);
      projectile.PhysicsController.AddForce(transform.forward * attackAttributes.FiringForce.Value, ForceMode.VelocityChange);
      projectile.SetDamage(attackAttributes.Damage);

      vfxPool.EnablePooledObject(attackAttributes.FiringVfx, fireZone.position, transform.rotation);

      --currentAmmo.Value;

      fireCountdown.Value += attackAttributes.AttackRate.Value;

      firingIsQueued = false;
    }

    private void OnAmmoChange()
    {
      // Ammo added
      if (attackAttributes.AmmoCount.Value > previousAmmoCount)
      {
        int diff = (int)attackAttributes.AmmoCount.Value - previousAmmoCount;
        currentAmmo.Value += diff;
      }

      // Ammo removed
      else
      {
        currentAmmo.Value = Mathf.Clamp(currentAmmo.Value, 0, (int)attackAttributes.AmmoCount.Value);
      }

      previousAmmoCount = (int)attackAttributes.AmmoCount.Value;
    }

    public void YouMayFireIfReady()
    {
      if (CanFire) QueueFiring();
    }

    public void QueueFiring()
    {
      // Queue firing which will be executed in update
      if (fireCountdown.Value <= attackAttributes.TimeToQueueFiring.Value)
        firingIsQueued = true;
    }
  }
}
