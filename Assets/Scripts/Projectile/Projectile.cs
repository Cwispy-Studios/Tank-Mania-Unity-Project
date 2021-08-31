using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Projectile
{
  using Combat;
  using Stats;
  using Visuals;

  public class Projectile : MonoBehaviour
  {
    // Particle system/vfx to play when projectile impacts
    [SerializeField] private CFX_AutoDestructShuriken explosionVfx = null;

    // Used to turn the projectile invisible while leaving its vfx running
    private MeshRenderer meshRenderer;
    // Prevents OnEnable from running when being instantiated
    private bool disableOnEnabled = true;
    // Prevents multiple collisions and hits from being registered at once
    private bool disableProjectileCollisions = false;

    // Damage from the object firing the projectile
    private Damage damageInformation;

    // Cache accessible rigidbody
    [HideInInspector] public Rigidbody PhysicsController;

    // Below collections are cleared immediately after each calculation and can be reused over every projectile
    // Used to hold the results of spherecast, reduces garbage collection
    private static Collider[] splashCollisionResults = new Collider[50];
    // Used for caching objects in splash calculations to prevent checking the same object more than once
    private static HashSet<GameObject> splashedObjects = new HashSet<GameObject>();

    private void Awake()
    {
      meshRenderer = GetComponent<MeshRenderer>();
      PhysicsController = GetComponent<Rigidbody>();

      disableOnEnabled = true;

      explosionVfx.OnDeactivate += Deactivate;
    }

    private void OnEnable()
    {
      if (disableOnEnabled) { disableOnEnabled = false; return; }

      BulletEvents.BulletFired(this);
    }

    private void Update()
    {
      transform.LookAt(PhysicsController.position + PhysicsController.velocity);
    }

    private void OnCollisionEnter( Collision collision )
    {
      if (disableProjectileCollisions) return;

      HandleProjectileCollison(collision);
    }

    private void HandleProjectileCollison( Collision collision )
    {
      GameObject collisionObject = collision.gameObject;
      Vector3 collisionPoint = collision.GetContact(0).point;

      DamageUnitIfValid(collisionObject);
      HandleSplashDamageIfEnabled(collisionObject, collisionPoint);

      // Moves the explosion vfx to the point of contact and enables it
      explosionVfx.transform.position = collisionPoint;
      explosionVfx.gameObject.SetActive(true);

      // Deactivates physics of the projectile
      PhysicsController.detectCollisions = false;
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Discrete;
      PhysicsController.isKinematic = true;

      // Deactivates projectile from colliding again before physics is disabled
      disableProjectileCollisions = true;

      // Turns the projectile invisible
      meshRenderer.enabled = false;

      BulletEvents.BulletHit(this);
    }

    private void DamageUnitIfValid( GameObject collisionObject )
    {
      // Check if object has a health component so it can be damaged
      Damageable hitObjectHealth = collisionObject.GetComponent<Damageable>();
      Team projectileTeam = damageInformation.DamageFrom;

      // Also check if unit is from a different team, no friendly fire
      if (hitObjectHealth && hitObjectHealth.CanTakeDamageFromTeam(projectileTeam))
        hitObjectHealth.TakeDamage(damageInformation.DirectDamage.Value);
    }

    private void HandleSplashDamageIfEnabled( GameObject collisionObject, Vector3 collisionPoint )
    {
      SplashDamage splashDamage = damageInformation.SplashDamage;
      if (!splashDamage.HasSplashDamage) return;

      Team projectileTeam = damageInformation.DamageFrom;

      // If this object took a direct hit already, it should not take additional splash damage
      splashedObjects.Add(collisionObject);

      // 8 is enemy, 3 is player
      int opponentLayerMask = damageInformation.DamageFrom == Team.Player ? 1 << 8 : 1 << 3;

      // Find the number of objects within the splash radius, this counts all composite colliders
      int numHits = Physics.OverlapSphereNonAlloc(
        collisionPoint,
        splashDamage.Radius.Value,
        splashCollisionResults,
        opponentLayerMask,
        QueryTriggerInteraction.Ignore
        );
      // Prevents out of range
      numHits = Mathf.Clamp(numHits, 0, splashCollisionResults.Length);

      // Loop through each hit
      for (int i = 0; i < numHits; ++i)
      {
        // Retrieve from attached rigidbody and not from the collision component
        // to check only the main gameobject and not all its colliders
        Rigidbody splashedRigidbody = splashCollisionResults[i].attachedRigidbody;
        GameObject splashedObject = splashedRigidbody.gameObject;

        // Check if object has already been searched
        if (!splashedObjects.Contains(splashedObject))
        {
          // Do not check this object again
          splashedObjects.Add(splashedObject);

          // Check if object has a health component and belongs to opposing team
          Damageable splashedObjectHealth = splashedObject.GetComponent<Damageable>();

          // Object has health, then check the team
          if (splashedObjectHealth && splashedObjectHealth.CanTakeDamageFromTeam(projectileTeam))
          {
            float splashDamageDealt = CalculateSplashDamage(splashedRigidbody, collisionPoint);          

            splashedObjectHealth.TakeDamage(splashDamageDealt);
          }
        }

        splashCollisionResults[i] = null;
      }

      splashedObjects.Clear();
    }

    private float CalculateSplashDamage( Rigidbody splashedRigidbody, Vector3 collisionPoint )
    {
      SplashDamage splashDamage = damageInformation.SplashDamage;

      // Object is eligible to be damaged from splash damage, calculate damage
      float baseSplashDamage =
        damageInformation.DirectDamage.Value * splashDamage.DamagePercentage.Value;
      float splashDamageDealt = baseSplashDamage;

      // If there is rolloff from radius, do further calculations
      if (splashDamage.HasSplashDamageRolloff)
      {
        Vector3 closestPointOfContact = splashedRigidbody.ClosestPointOnBounds(collisionPoint);
        float sqrDistance = Vector3.SqrMagnitude(collisionPoint - closestPointOfContact);

        // NOTE: Will need caching inside DamageInformation if it causes performance problems
        float minRadius = splashDamage.Radius.Value * splashDamage.MinRadiusPercentageRolloff.Value;
        float sqrMinRadius = minRadius * minRadius;

        float maxRadius = splashDamage.Radius.Value * splashDamage.MaxRadiusPercentageRolloff.Value;
        float sqrMaxRadius = maxRadius * maxRadius;

        if (sqrDistance <= sqrMinRadius) splashDamageDealt = baseSplashDamage;

        else if (sqrDistance >= sqrMaxRadius)
          splashDamageDealt = baseSplashDamage * splashDamage.MaxRadiusDamagePercentageRolloff.Value;

        else
        {
          float t = (sqrDistance - sqrMinRadius) / (sqrMaxRadius - sqrMinRadius);

          splashDamageDealt *= Mathf.Lerp(
            splashDamage.MinRadiusDamagePercentageRolloff.Value,
            splashDamage.MaxRadiusDamagePercentageRolloff.Value,
            t);
        }
      }

      return splashDamageDealt;
    }

    private void Deactivate()
    {
      // Reenables physics
      PhysicsController.isKinematic = false;
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Continuous;
      PhysicsController.velocity = Vector3.zero;
      PhysicsController.detectCollisions = true;

      // Turns the projectile visible
      meshRenderer.enabled = true;

      // Resets damage information
      damageInformation = null;

      // Return to object pool
      gameObject.SetActive(false);

      disableProjectileCollisions = false;
    }

    public void SetDamage( Damage damage )
    {
      damageInformation = damage;
    }
  }
}
