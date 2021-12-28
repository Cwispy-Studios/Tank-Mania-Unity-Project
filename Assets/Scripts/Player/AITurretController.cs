using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Combat;
  using Stats;

  [RequireComponent(typeof(TurretHub))]
  public class AITurretController : MonoBehaviour
  {
    [SerializeField] private AITurretStats aiTurretStats;
    [Tooltip("Ordered hard criterias that AI will prefer.")]
    [SerializeField] private List<UnitProperties> targetingCriterias;

    // Refresh rate to check for targets is the same for every instance.
    private static float TargetRefreshInterval = 0.4f;

    /// <summary>
    /// Stores multiple lists of targets that the turret can rotate to face. 
    /// These lists are sorted based on the targeting criteria order. The highest priority is at the top.
    /// </summary>
    private List<List<Rigidbody>> validTargetsSortedByCriterias = new List<List<Rigidbody>>();

    // Cache components
    private SphereCollider detectionCollider;
    private TargetsFinder targetsFinder;
    private TurretHub turretHub;
    private GunController gun;

    /// <summary>
    /// The highest priority target found that the turret can rotate to.
    /// </summary>
    private Rigidbody selectedTarget = null;

    private float targetRefreshTimer = 0f;

    private void Awake()
    {
      detectionCollider = GetComponent<SphereCollider>();
      targetsFinder = GetComponent<TargetsFinder>();
      turretHub = GetComponentInChildren<TurretHub>();
      gun = GetComponentInChildren<GunController>();

      // Add a list of valid targets for every targeting criteria, +1 for a list that accepts everything else
      for (int i = 0; i <= targetingCriterias.Count; ++i)
      {
        validTargetsSortedByCriterias.Add(new List<Rigidbody>());
      }
    }

    private void OnEnable()
    {
      aiTurretStats.DetectionRange.OnStatUpgrade += AdjustDetectionSphereRadius;
      AdjustDetectionSphereRadius();
    }

    private void OnDisable()
    {
      aiTurretStats.DetectionRange.OnStatUpgrade -= AdjustDetectionSphereRadius;
    }

    private void Update()
    {
      targetRefreshTimer += Time.deltaTime;

      if (targetRefreshTimer >= TargetRefreshInterval)
      {
        // First refresh and sort the targets that can be hit by the turret by criteria
        int numberOfValidTargets = RefreshAndSortValidTargets();

        // If there were any valid targets found, select the highest priority one
        if (numberOfValidTargets > 0) SelectHighestPriorityTarget();

        targetRefreshTimer = 0f;
      }

      // If there is a selected target, turret should rotate to aim at it every frame
      if (selectedTarget != null) AimAtTarget();
    }

    /// <summary>
    /// Retrieves the list of targets in range from the TargetFinder, checks which the turret can rotate to face,
    /// and sorts the valid ones according to the targeting criterias set.
    /// </summary>
    /// <returns>
    /// The number of valid targets found (targets which the turret can rotate to face).
    /// </returns>
    private int RefreshAndSortValidTargets()
    {
      List<Rigidbody> targetsInRange = new List<Rigidbody>(targetsFinder.TargetsInRange);
      foreach (List<Rigidbody> targetsPerCriteria in validTargetsSortedByCriterias) targetsPerCriteria.Clear();
      selectedTarget = null;

      int numberOfValidTargets = 0;

      // Loop through every target in range...
      for (int i = 0; i < targetsInRange.Count; ++i)
      {
        // Validate that the target is not too near the turret and the turret and gun can rotate to face it.
        Rigidbody target = targetsInRange[i];

        float sqrMag = Vector3.SqrMagnitude(target.position - transform.position);
        float minDetectionRange = aiTurretStats.MinDetectionRange.Value;
        // First check if target is not too close to the turret
        bool targetNotInMinDetectionRange = minDetectionRange > 0f && sqrMag <= minDetectionRange * minDetectionRange;

        if (!targetNotInMinDetectionRange) continue;

        // Second, check that turret can rotate to face target
        bool targetIsInRotationLimit = turretHub.CheckTargetInRotationRange(target);

        if (!targetIsInRotationLimit) continue;

        // Loop through every targeting criteria...
        for (int criteriaIndex = 0; criteriaIndex <= targetingCriterias.Count; ++criteriaIndex)
        {
          // and check if the target's unit properties matches the criteria.
          // No need to check for null since this is already checked by the TargetFinder
          Damageable damageable = target.GetComponent<Damageable>();

          // As it is right now, we only need to sort targets that matches the first criteria found
          // but I am sorting them already cause we will likely need it in the future.

          bool targetMatchesCriteria;

          // Check if index is within range of list of criterias
          if (criteriaIndex < targetingCriterias.Count)
          {
            targetMatchesCriteria = targetingCriterias[criteriaIndex].IsHardMatchWith(damageable.UnitProperties);
          }

          // If out of range, that means every other criteria has failed and this target belongs to the lowest priority, auto match.
          else targetMatchesCriteria = true;

          if (targetMatchesCriteria)
          {
            ++numberOfValidTargets;
            validTargetsSortedByCriterias[criteriaIndex].Add(target);
          }
        }
      }

      return numberOfValidTargets;
    }

    /// <summary>
    /// Finds and assigns the target that matches the highest targeting criteria found.
    /// If there are more than 1 target that matches, selects the closest one to the turret.
    /// </summary>
    private void SelectHighestPriorityTarget()
    {
      // Loop through the list of valid targets by criteria
      for (int i = 0; i < validTargetsSortedByCriterias.Count; ++i)
      {
        List<Rigidbody> targetsInCriteria = validTargetsSortedByCriterias[i];

        // Are there any targets that match this criteria?
        if (targetsInCriteria.Count > 0)
        {
          // If only 1 object, by default that object will be selected
          if (targetsInCriteria.Count == 1) selectedTarget = targetsInCriteria[0];

          // If there are more than 1, get the object closest to the turret
          else
          {
            float smallestSqDist = Mathf.Infinity;

            for (int j = 0; j < targetsInCriteria.Count; ++j)
            {
              Rigidbody target = targetsInCriteria[j];
              float sqDist = Vector3.SqrMagnitude(target.position - gun.FireZone.position);

              if (sqDist < smallestSqDist)
              {
                smallestSqDist = sqDist;
                selectedTarget = target;
              }
            }
          }

          // Return since we only need to select 1 object to aim at
          return;
        }
      }
    }

    /// <summary>
    /// Rotates the turret to face its selected target.
    /// </summary>
    private void AimAtTarget()
    {
      // First make sure that the target can still be reached and get the angular distances to rotate
      if (turretHub.CheckTargetInRotationRange(selectedTarget, out float horizontalAngularDistance, out float verticalAngularDistance))
      {
        float horizontalRotationAmount = turretHub.RotateTurretByValue(horizontalAngularDistance);
        float verticalRotationAmount = turretHub.RotateGunByValue(verticalAngularDistance);

        // Since turret has already moved by the amount above, we want to find how much more it has left to rotate before it faces its target
        float remainingHorizontalAngularDistance = 
          Mathf.Abs(horizontalAngularDistance) - Mathf.Abs(horizontalRotationAmount);

        float remainingVerticalAngularDistance =
          Mathf.Abs(verticalAngularDistance) - Mathf.Abs(verticalRotationAmount);

        // If remaining horizontal and vertical angular distances are lower than the imprecision amount, turret can shoot at target
        bool turretIsFacingTarget = remainingHorizontalAngularDistance <= aiTurretStats.Imprecision.Value &&
          remainingVerticalAngularDistance <= aiTurretStats.Imprecision.Value;

        if (turretIsFacingTarget)
        {
          gun.YouMayFireIfReady();
        }
      }
    }

    /// <summary>
    /// Adjusts the sphere collider trigger based on the detection range of the turret.
    /// </summary>
    private void AdjustDetectionSphereRadius()
    {
      detectionCollider.radius = aiTurretStats.DetectionRange.Value;
    }
  }
}
