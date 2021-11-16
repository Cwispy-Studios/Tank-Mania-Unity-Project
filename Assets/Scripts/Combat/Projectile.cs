using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Poolers;
  using Stats;
  using Visuals;

  public class Projectile : MonoBehaviour
  {
    // Particle system/vfx to play when projectile impacts
    [SerializeField] private VfxParentDisabler impactVfxPrefab = null;

    [Header("Custom Gravity")]
    [SerializeField] private bool usesCustomGravity = false;
    [SerializeField, Range(0, -100f), Tooltip("Default gravity is -9.81")] 
    private float customGravityValue;

    private VfxPooler vfxPooler;

    // Prevents OnEnable from running when being instantiated
    private bool disableOnEnabled = true;
    // Prevents multiple collisions and hits from being registered at once
    private bool disableProjectileCollisions = false;

    // Damage from the object firing the projectile
    private Damage damageInformation;

    // Cache accessible rigidbody
    [HideInInspector] public Rigidbody PhysicsController;

    private void Awake()
    {
      vfxPooler = FindObjectOfType<VfxPooler>();

      PhysicsController = GetComponent<Rigidbody>();
      if (usesCustomGravity) PhysicsController.useGravity = false;

      disableOnEnabled = true;
    }

    private void OnEnable()
    {
      if (disableOnEnabled) { disableOnEnabled = false; return; }

      disableProjectileCollisions = false;
      BulletEvents.BulletFired(this);
    }

    private void FixedUpdate()
    {
      if (usesCustomGravity) PhysicsController.AddForce(0f, customGravityValue, 0f, ForceMode.Acceleration);
    }

    private void Update()
    {
      transform.LookAt(PhysicsController.position + PhysicsController.velocity);
    }

    private void OnCollisionEnter( Collision collision )
    {
      ProjectileCollision(collision);
    }

    private void ProjectileCollision( Collision collision )
    {
      if (disableProjectileCollisions) return;

      Vector3 collisionPoint = collision.GetContact(0).point;
      EnableImpactVFX(collisionPoint);

      // Damage the object it collided with
      GameObject collisionObject = collision.gameObject;
      ProjectileDamage(collisionObject);

      // If projectile has splash damage, also do splash damage
      if (damageInformation.HasSplashDamage)
      {
        ProjectileExplosion(collisionObject, collisionPoint);
      }

      BulletEvents.BulletHit(this);

      Deactivate();
    }

    public void ProjectileTrigger()
    {
      if (disableProjectileCollisions) return;

      EnableImpactVFX(transform.position);

      // If projectile has splash damage, also do splash damage
      if (damageInformation.HasSplashDamage)
      {
        ProjectileExplosion(null, transform.position);
      }

      BulletEvents.BulletHit(this);

      Deactivate();
    }

    /// <summary>
    /// Does direct damage to a single target
    /// </summary>
    /// <param name="objectToDamage"></param>
    public void ProjectileDamage( GameObject objectToDamage )
    {
      damageInformation.DamageObject(objectToDamage);
    }

    /// <summary>
    /// Does direct damage to a list of targets
    /// </summary>
    /// <param name="collision"></param>
    public void ProjectileDamage( IEnumerable<GameObject> objectsToDamage )
    {
      foreach (GameObject objectToDamage in objectsToDamage) ProjectileDamage(objectToDamage);
    }

    /// <summary>
    /// Does splash damage at a target point
    /// </summary>
    /// <param name="explosionPoint"></param>
    public void ProjectileExplosion( GameObject collisionObject, Vector3 explosionPoint )
    {
      damageInformation.SplashDamageOnPoint(collisionObject, explosionPoint);
    }

    private void EnableImpactVFX( Vector3 location )
    {
      vfxPooler.EnablePooledObject(impactVfxPrefab, location, Quaternion.identity);
    }

    private void Deactivate()
    {
      PhysicsController.velocity = Vector3.zero;
      PhysicsController.angularVelocity = Vector3.zero;

      // Resets damage information
      damageInformation = null;

      // Return to object pool
      gameObject.SetActive(false);

      disableProjectileCollisions = true;
    }

    public void SetDamage( Damage damage )
    {
      damageInformation = damage;
    }
  }
}
