using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Combat;
  using Stats;

  public class AITurretController : MonoBehaviour
  {
    [SerializeField] private AITurretStats aiTurretStats;

    private static float s_targetRefreshInterval = 0.5f;

    private List<Rigidbody> targetsInRotationLimit = new List<Rigidbody>();

    private SphereCollider sphereCollider;
    private TargetFinder targetFinder;
    private TurretHub turretHub;
    private GunController gun;
    private Transform fireZone;

    private Rigidbody selectedTarget = null;

    //private bool isActive = true;
    private float targetRefreshTimer = 0f;

    private void Awake()
    {
      sphereCollider = GetComponent<SphereCollider>();
      targetFinder = GetComponent<TargetFinder>();
      turretHub = GetComponentInChildren<TurretHub>();
      gun = GetComponentInChildren<GunController>();
      fireZone = gun.FireZone;

      aiTurretStats.DetectionRange.OnStatUpgrade += AdjustDetectionSphereRadius;
      AdjustDetectionSphereRadius();
    }

    private void Update()
    {
      targetRefreshTimer += Time.deltaTime;

      if (targetRefreshTimer >= s_targetRefreshInterval)
      {
        RefreshTargets();

        if (targetsInRotationLimit.Count > 0) SelectTarget();

        targetRefreshTimer = 0f;
      }

      if (selectedTarget != null) AimAtTarget();
    }

    private void RefreshTargets()
    {
      List<Rigidbody> targetsInRange = new List<Rigidbody>(targetFinder.TargetsInRange);
      targetsInRotationLimit.Clear();
      selectedTarget = null;

      // Loop through every target in range...
      for (int i = 0; i < targetsInRange.Count; ++i)
      {
        Rigidbody target = targetsInRange[i];

        bool targetIsInRotationLimit = CheckTargetCanBeHit(target);

        if (targetIsInRotationLimit) targetsInRotationLimit.Add(target);
      }
    }

    private bool CheckTargetCanBeHit( Rigidbody target )
    {
      return CheckTargetCanBeHit(target, out _, out _);
    }

    private bool CheckTargetCanBeHit( Rigidbody target, out float horizontalAngleDistance, out float verticalAngleDistance )
    {
      horizontalAngleDistance = 0f;
      verticalAngleDistance = 0f;

      float minDetectionRange = aiTurretStats.MinDetectionRange.Value;

      // First check if target is too close to the turret
      if (minDetectionRange > 0f && Vector3.SqrMagnitude(target.position - transform.position) <= minDetectionRange * minDetectionRange) 
        return false;

      bool targetIsInRotationLimit = true;

      horizontalAngleDistance = GetTurretAngleDifferenceFromTarget(target);

      // Check if turret can rotate horizontally (y-axis) to face the target
      if (turretHub.RotationLimits.HasYLimits)
      { 
        // Check if turret can rotate by this much
        float targetAngle = turretHub.TurretRotationValue + horizontalAngleDistance;

        targetIsInRotationLimit = targetAngle >= turretHub.RotationLimits.MinYRot && targetAngle <= turretHub.RotationLimits.MaxYRot;
      }

      // Check if gun can rotate vertically (x-axis) to face the target
      if (targetIsInRotationLimit && turretHub.RotationLimits.HasXLimits)
      {
        verticalAngleDistance = GetGunAngleDifferenceFromTarget(target, horizontalAngleDistance);

        // Check if turret can rotate by this much
        float targetAngle = turretHub.GunRotationValue + verticalAngleDistance;
        targetIsInRotationLimit = targetAngle >= turretHub.RotationLimits.MinXRot && targetAngle <= turretHub.RotationLimits.MaxXRot;
      }

      return targetIsInRotationLimit;
    }

    private float GetTurretAngleDifferenceFromTarget( Rigidbody target )
    {
      // Find the angle the turret must rotate to face the target
      // To do this, we must "rotate" the target around the turret until it is in line of sight of the fire zone direction
      // Since the fire zone position and rotation may not be the same as the turret, we must perform additional calculations
      // Solution taken from:
      // https://gamedev.stackexchange.com/questions/147714/rotate-object-such-that-a-child-object-is-facing-a-target-3d

      // TODO (Optimisation): Check if there is any offset, then perform simplified calculations if there is no offset

      // Find the closest point from the turret to the fire direction ray
      // https://stackoverflow.com/questions/51905268/how-to-find-closest-point-on-line
      Vector3 turretPosition = turretHub.Turret.position;
      Vector3 firePosition = gun.FireZone.position;
      Vector3 fireDirectionEnd = firePosition + gun.FireZone.forward;

      // Projected vector of offset direction of fire zone from turret onto fire direction
      Vector3 projectedVector = Vector3.Project(turretPosition - firePosition, fireDirectionEnd - firePosition);
      // Closest point from the turret to the fire direction
      Vector3 closestPoint = firePosition + projectedVector;

      // Pythogras theorem to get distance to expected position
      float SqrMagToTarget = Vector3.SqrMagnitude(turretPosition - target.position);
      float SqrMagToClosestPoint = Vector3.SqrMagnitude(turretPosition - closestPoint);
      float distToExpectedPosition = Mathf.Sqrt(SqrMagToTarget - SqrMagToClosestPoint);

      // Expected position is from the closest point moved forward by the fire direction 
      Vector3 expectedTargetPosition = closestPoint + Vector3.Normalize(gun.FireZone.forward) * distToExpectedPosition;

      // Local position of the target to the turret
      Vector3 targetLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(target.position);
      // Local position of where the projectile leaves the gun to the turret
      Vector3 expectedLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(expectedTargetPosition);

      // Remove y-axis
      Vector3 from = targetLocalPositionFromTurret;
      from.y = 0f;
      Vector3 to = expectedLocalPositionFromTurret;
      to.y = 0f;

      // Since we are not using y-axis, axis just points downward locally
      float angularDistance = Vector3.SignedAngle(from, to, Vector3.down);

      return angularDistance;
    }

    private float GetGunAngleDifferenceFromTarget( Rigidbody target, float turretRotation )
    {
      // If we get here, this assumes the turret can rotate horizontally to face the target

      // Find the rotation vector to rotate the gun direction and offset, this is rotated by the hub's base rotation taken from the slot's rotation.
      Vector3 rotationAngles = transform.rotation * new Vector3(0f, -turretRotation, 0f);
      // Rotate the target about the pivot point
      Vector3 horizontallyRotatedTargetPosition = MathHelper.RotatePointAroundPivot(target.position, turretHub.Turret.position, rotationAngles);

      // Find the closest point from the turret to the fire direction ray
      // https://stackoverflow.com/questions/51905268/how-to-find-closest-point-on-line
      Vector3 gunPosition = turretHub.Gun.position;
      Vector3 firePosition = gun.FireZone.position;
      Vector3 fireDirectionEnd = firePosition + gun.FireZone.forward;

      // Projected vector of offset direction of fire zone from turret onto fire direction
      Vector3 projectedVector = Vector3.Project(gunPosition - firePosition, fireDirectionEnd - firePosition);
      // Closest point from the turret to the fire direction
      Vector3 closestPoint = firePosition + projectedVector;

      // Pythogras theorem to get distance to expected position
      float SqrMagToTarget = Vector3.SqrMagnitude(gunPosition - horizontallyRotatedTargetPosition);
      float SqrMagToClosestPoint = Vector3.SqrMagnitude(gunPosition - closestPoint);
      float distToExpectedPosition = Mathf.Sqrt(SqrMagToTarget - SqrMagToClosestPoint);

      // Expected position is from the closest point moved forward by the fire direction 
      Vector3 expectedTargetPosition = closestPoint + Vector3.Normalize(gun.FireZone.forward) * distToExpectedPosition;

      // Local position of the target to the turret
      Vector3 targetLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(horizontallyRotatedTargetPosition);
      // Local position of where the projectile leaves the gun to the turret
      Vector3 expectedLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(expectedTargetPosition);

      // Since we are not using x-axis, axis just points leftwards locally
      float angularDistance = Vector3.SignedAngle(targetLocalPositionFromTurret, expectedLocalPositionFromTurret, Vector3.left);

      return angularDistance;
    }

    private void SelectTarget()
    {
      float smallestSqDist = Mathf.Infinity;

      for (int i = 0; i < targetsInRotationLimit.Count; ++i)
      {
        Rigidbody target = targetsInRotationLimit[i];
        float sqDist = Vector3.SqrMagnitude(target.position - gun.FireZone.position);

        if (sqDist < smallestSqDist)
        {
          smallestSqDist = sqDist;
          selectedTarget = target;
        }
      }
    }

    private void AimAtTarget()
    {
      if (CheckTargetCanBeHit(selectedTarget, out float horizontalAngularDistance, out float verticalAngularDistance))
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

    private void AdjustDetectionSphereRadius()
    {
      sphereCollider.radius = aiTurretStats.DetectionRange.Value;
    }
  }
}
