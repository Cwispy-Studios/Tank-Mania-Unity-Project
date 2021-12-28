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
    // Below collections are cleared immediately after each calculation and can be reused over every damager
    // Used to hold the results of spherecast, reduces garbage collection
    private static Collider[] splashCollisionResults = new Collider[100];
    // Used for caching objects in splash calculations to prevent checking the same object more than once
    private static HashSet<GameObject> splashedObjects = new HashSet<GameObject>();

    /// <summary>
    /// Checks if a target unit can be damaged and damages them.
    /// </summary>
    /// <param name="damage">
    /// The damage information.
    /// </param>
    /// <param name="collisionObject">
    /// Target object to damage.
    /// </param>
    public static void DamageObject( this Damage damage, GameObject collisionObject, bool isDudCollision )
    {
      Damageable damageable = collisionObject.GetComponent<Damageable>();

      if (damageable != null)
      {
        float damageAmount = damage.DirectDamage.Value;
        if (isDudCollision) damageAmount *= damage.DudCollisionDamagePercentage.Value;

        damageable.TakeDamage(damageAmount);
      }
    }

    /// <summary>
    /// If splash damage is enabled on Damage instance, retrieves all objects around a point of damage.
    /// </summary>
    /// <param name="damage">
    /// The damage information.
    /// </param>
    /// <param name="collisionObject"> 
    /// Object that was directly hit by the Damage. Can be null.
    /// </param>
    /// <param name="collisionPoint">
    /// The point where splash damage is emitted from.
    /// </param>
    public static void SplashDamageOnPoint( this Damage damage, GameObject collisionObject, Vector3 collisionPoint, int layerMask )
    {
      if (!damage.HasSplashDamage) return;

      // If this object took a direct hit already, it should not take additional splash damage
      splashedObjects.Add(collisionObject);

      // Find the number of objects within the splash radius, this counts all composite colliders
      int numHits = Physics.OverlapSphereNonAlloc(
        collisionPoint,
        damage.SplashRadius.Value,
        splashCollisionResults,
        layerMask,
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

          Damageable damageable = splashedObject.GetComponent<Damageable>();

          if (damageable != null)
          {
            float splashDamageDealt = damage.CalculateSplashDamage(splashedRigidbody, collisionPoint);
            damageable.TakeDamage(splashDamageDealt);
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
