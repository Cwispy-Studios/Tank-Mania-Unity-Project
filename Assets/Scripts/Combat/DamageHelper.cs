using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  /// <summary>
  /// Extension class for Damage that handles all damage calculations
  /// </summary>
  public static class DamageHelper
  {
    // Below collections are cleared immediately after each calculation and can be reused over every projectile
    // Used to hold the results of spherecast, reduces garbage collection
    private static Collider[] splashCollisionResults = new Collider[100];
    // Used for caching objects in splash calculations to prevent checking the same object more than once
    private static HashSet<GameObject> splashedObjects = new HashSet<GameObject>();

    /// <summary>
    /// Checks if a target unit can be damaged and retrieve the Damageable component.
    /// </summary>
    /// <param name="damage">
    /// The damage information.
    /// </param>
    /// <param name="damageFrom">
    /// The team the damage is coming from.
    /// </param>
    /// <param name="collisionObject">
    /// Object to check if it can be damaged.
    /// </param>
    /// <param name="hitObjectHealth">
    /// The Damageable component to retrieve.
    /// </param>
    /// <returns>
    /// Whether the object can be damaged.
    /// </returns>
    public static bool CheckIfDamageable( this Damage damage, Team damageFrom, GameObject collisionObject, out Damageable hitObjectHealth )
    {
      // Check if object has a health component so it can be damaged
      hitObjectHealth = collisionObject.GetComponent<Damageable>();

      return hitObjectHealth && hitObjectHealth.CanTakeDamageFromTeam(damageFrom);
    }

    /// <summary>
    /// Checks if a target unit can be damaged.
    /// </summary>
    /// <param name="damage">
    /// The damage information.
    /// </param>
    /// <param name="damageFrom">
    /// The team the damage is coming from.
    /// </param>
    /// <param name="collisionObject">
    /// Object to check if it can be damaged.
    /// </param>
    /// <returns>
    /// Whether the object can be damaged.
    /// </returns>
    public static bool CheckIfDamageable( this Damage damage, Team damageFrom, GameObject collisionObject )
    {
      return CheckIfDamageable(damage, damageFrom, collisionObject, out _);
    }

    /// <summary>
    /// Checks if a target unit can be damaged and damages them.
    /// </summary>
    /// <param name="damage">
    /// The damage information.
    /// </param>
    /// <param name="damageFrom">
    /// The team the damage is coming from.
    /// </param>
    /// <param name="collisionObject">
    /// Target object to damage.
    /// </param>
    public static void DamageObject( this Damage damage, Team damageFrom, GameObject collisionObject, bool isDudCollision )
    {
      // Also check if unit is from a different team, no friendly fire
      if (CheckIfDamageable(damage, damageFrom, collisionObject, out Damageable hitObjectHealth))
      {
        float damageAmount = damage.DirectDamage.Value;
        if (isDudCollision) damageAmount *= damage.DudCollisionDamagePercentage.Value;

        hitObjectHealth.TakeDamage(damageAmount);
      }
    }

    /// <summary>
    /// If splash damage is enabled on Damage instance, retrieves all objects around a point of damage.
    /// </summary>
    /// <param name="damage">
    /// The damage information.
    /// </param>
    /// <param name="damageFrom">
    /// The team the damage is coming from.
    /// </param>
    /// <param name="collisionObject"> 
    /// Object that was directly hit by the Damage. Can be null.
    /// </param>
    /// <param name="collisionPoint">
    /// The point where splash damage is emitted from.
    /// </param>
    public static void SplashDamageOnPoint( this Damage damage, Team damageFrom, GameObject collisionObject, Vector3 collisionPoint )
    {
      if (!damage.HasSplashDamage) return;

      // If this object took a direct hit already, it should not take additional splash damage
      splashedObjects.Add(collisionObject);

      // 8 is enemy, 3 is player
      int opponentLayerMask = damageFrom == Team.Player ? 1 << 8 : 1 << 3;

      // Find the number of objects within the splash radius, this counts all composite colliders
      int numHits = Physics.OverlapSphereNonAlloc(
        collisionPoint,
        damage.SplashRadius.Value,
        splashCollisionResults,
        opponentLayerMask,
        QueryTriggerInteraction.Ignore
        );
      // Prevents out of range
      numHits = Mathf.Clamp(numHits, 0, splashCollisionResults.Length);

      // Loop through each hit
      for (int i = 0; i < numHits; ++i)
      {
        // If object is already disabled, do not check
        if (!splashCollisionResults[i].gameObject.activeInHierarchy) continue;

        // Retrieve from attached rigidbody and not from the collision component
        // to check only the main gameobject and not all its colliders
        Rigidbody splashedRigidbody = splashCollisionResults[i].attachedRigidbody;
        GameObject splashedObject = splashedRigidbody.gameObject;

        // Check if object has already been searched
        if (!splashedObjects.Contains(splashedObject))
        {
          // Do not check this object again
          splashedObjects.Add(splashedObject);

          // Object has health, then check the team
          if (damage.CheckIfDamageable(damageFrom, splashedObject, out Damageable splashedObjectHealth))
          {
            float splashDamageDealt = damage.CalculateSplashDamage(splashedRigidbody, collisionPoint);
            splashedObjectHealth.TakeDamage(splashDamageDealt);
          }
        }

        splashCollisionResults[i] = null;
      }

      splashedObjects.Clear();
    }

    /// <summary>
    /// Calculates the amount of splash damage to deal to a rigidbody based on its distance to the collision point of the splash damage.
    /// </summary>
    /// <param name="damage">
    /// The damage information
    /// </param>
    /// <param name="splashedRigidbody">
    /// Rigidbody to check.
    /// </param>
    /// <param name="collisionPoint">
    /// Point of collision of splash damage.
    /// </param>
    /// <returns>
    /// The amount of splash damage the rigidbody should receive.
    /// </returns>
    public static float CalculateSplashDamage( this Damage damage, Rigidbody splashedRigidbody, Vector3 collisionPoint )
    {
      // Object is eligible to be damaged from splash damage, calculate damage
      float baseSplashDamage =
        damage.DirectDamage.Value * damage.SplashDamagePercentage.Value;
      float splashDamageDealt = baseSplashDamage;

      // If there is rolloff from radius, do further calculations
      if (damage.HasSplashDamageRolloff)
      {
        Vector3 closestPointOfContact = splashedRigidbody.ClosestPointOnBounds(collisionPoint);
        float sqrDistance = Vector3.SqrMagnitude(collisionPoint - closestPointOfContact);

        // NOTE: Calculations can be cached inside DamageInformation for optimisation
        float minRadius = damage.SplashRadius.Value * damage.SplashMinRadiusPercentageRolloff.Value;
        float sqrMinRadius = minRadius * minRadius;

        float maxRadius = damage.SplashRadius.Value * damage.SplashMaxRadiusPercentageRolloff.Value;
        float sqrMaxRadius = maxRadius * maxRadius;

        // Within min damage radius
        if (sqrDistance <= sqrMinRadius) splashDamageDealt = baseSplashDamage;

        // Outside of max damage radius
        else if (sqrDistance >= sqrMaxRadius)
          splashDamageDealt = baseSplashDamage * damage.SplashMaxRadiusDamagePercentageRolloff.Value;

        // Within variable damage radius
        else
        {
          float t = (sqrDistance - sqrMinRadius) / (sqrMaxRadius - sqrMinRadius);

          splashDamageDealt *= Mathf.Lerp(
            damage.SplashMinRadiusDamagePercentageRolloff.Value,
            damage.SplashMaxRadiusDamagePercentageRolloff.Value,
            t);
        }
      }

      return splashDamageDealt;
    }
  }
}
