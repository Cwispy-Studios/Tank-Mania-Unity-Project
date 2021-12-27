using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Poolers;
  using Stats;
  using Visuals;

  [RequireComponent(typeof(UnitTeam))]
  public class Projectile : MonoBehaviour
  {
    [Tooltip("Units without a soft match with these properties do weakened damage and do not do splash damage.")]
    [SerializeField] private UnitProperties effectiveAgainst;
    [Tooltip("Particle system/vfx to play when projectile impacts normally")]
    [SerializeField] private VfxParentDisabler impactVfxPrefab = null;
    [Tooltip("Particle system/vfx to play when projectile impacts something it is not effective against")]
    [SerializeField] private VfxParentDisabler dudImpactVfxPrefab = null;

    [Header("Custom Gravity")]
    [SerializeField] private bool usesCustomGravity = false;
    [SerializeField, Range(0, -100f), Tooltip("Default gravity is -9.81")] 
    private float customGravityValue;

    private VfxPool vfxPool;

    // Prevents OnEnable from running when being instantiated
    private bool disableOnEnabled = true;
    // Prevents multiple collisions and hits from being registered at once
    private bool disableProjectileCollisions = false;

    // Damage from the object firing the projectile
    private Damage damageInformation;
    private UnitTeam unitTeam;

    // Cache accessible rigidbody
    [HideInInspector] public Rigidbody PhysicsController;

    private void Awake()
    {
      vfxPool = FindObjectOfType<VfxPool>();
      unitTeam = GetComponent<UnitTeam>();

      PhysicsController = GetComponent<Rigidbody>();
      if (usesCustomGravity) PhysicsController.useGravity = false;

      disableOnEnabled = true;

      if (gameObject.layer < 10 || gameObject.layer > 13) 
        Debug.LogWarning($"Warning, invalid layer mask {gameObject.layer} in projectile: ", this);
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

    /// <summary>
    /// Called when OnCollisionEnter is triggered, handles collision of the projectile
    /// </summary>
    /// <param name="collision">
    /// The object the projectile collided with.
    /// </param>
    private void ProjectileCollision( Collision collision )
    {
      if (disableProjectileCollisions) return;

      bool isDudCollision = CheckIfIsDudCollision(collision);

      Vector3 collisionPoint = collision.GetContact(0).point;
      EnableImpactVFX(collisionPoint, isDudCollision);

      // Damage the object it collided with
      GameObject collisionObject = collision.gameObject;
      ProjectileDamage(collisionObject, isDudCollision);

      if (!isDudCollision && damageInformation.HasSplashDamage)
      {
        ProjectileExplosion(collisionObject, collisionPoint);
      }

      BulletEvents.BulletHit(this);

      Deactivate();
    }

    /// <summary>
    /// Checks if the projectile collided with an object that is not of a preferred type.
    /// </summary>
    /// <param name="collision">
    /// The object the projectile collided with.
    /// </param>
    /// <returns>
    /// Whether the collision was a dud.
    /// </returns>
    private bool CheckIfIsDudCollision( Collision collision )
    {
      // If there is no effective type, it will never be a dud collision
      if (effectiveAgainst == 0 || effectiveAgainst == (UnitProperties)~0) return false;

      Damageable damageable = collision.gameObject.GetComponent<Damageable>();

      // If object cannot be damaged (hitting terrain), default to dud collision
      if (damageable == null) return true;

      // Dud collision if none of the unit's properties match against the effectiveAgainst
      return !effectiveAgainst.IsSoftMatchWith(damageable.UnitProperties);
    }

    /// <summary>
    /// Called from a Triggerer class. Guranteed non-dud collision.
    /// </summary>
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
    private void ProjectileDamage( GameObject objectToDamage, bool isDudCollision = false )
    {
      damageInformation.DamageObject(objectToDamage, isDudCollision);
    }

    /// <summary>
    /// Does direct damage to a list of targets
    /// </summary>
    /// <param name="collision"></param>
    private void ProjectileDamage( IEnumerable<GameObject> objectsToDamage )
    {
      foreach (GameObject objectToDamage in objectsToDamage) ProjectileDamage(objectToDamage);
    }

    /// <summary>
    /// Does splash damage at a target point
    /// </summary>
    /// <param name="explosionPoint"></param>
    private void ProjectileExplosion( GameObject collisionObject, Vector3 explosionPoint )
    {
      damageInformation.SplashDamageOnPoint(collisionObject, explosionPoint, unitTeam.OpponentLayerMask);
    }

    /// <summary>
    /// Enables the impact VFX from the VFX Pooler.
    /// </summary>
    /// <param name="location">
    /// Position where the VFX should occur.
    /// </param>
    /// <param name="isDudCollision">
    /// Whether to use the normal impact VFX or dud impact VFX.
    /// </param>
    private void EnableImpactVFX( Vector3 location, bool isDudCollision = false )
    {
      // TODO: REMOVE WHEN NOT DEBUGGING
      if (dudImpactVfxPrefab == null && impactVfxPrefab == null) return;

      VfxParentDisabler vfxParentDisabler = isDudCollision ? dudImpactVfxPrefab : impactVfxPrefab;
      vfxPool.EnablePooledObject(vfxParentDisabler, location, Quaternion.identity);
    }

    /// <summary>
    /// Disables the projectile so it is available in the pooler.
    /// </summary>
    private void Deactivate()
    {
      // Resets physics so it does not carry over when it is reenabled.
      PhysicsController.velocity = Vector3.zero;
      PhysicsController.angularVelocity = Vector3.zero;

      // Resets damage information
      damageInformation = null;

      // Return to object pool
      gameObject.SetActive(false);

      disableProjectileCollisions = true;
    }

    /// <summary>
    /// Passes damage information from the firer to the projectile.
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage( Damage damage )
    {
      damageInformation = damage;
    }
  }
}
