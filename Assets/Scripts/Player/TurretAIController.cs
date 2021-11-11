using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Combat;

  public class TurretAIController : MonoBehaviour
  {
    private static float s_targetRefreshInterval = 0.5f;

    private List<Rigidbody> targetsInRotationLimit = new List<Rigidbody>();

    private TargetFinder targetFinder;
    private TurretHub turretHub;
    private GunController gun;
    private Transform fireZone;

    private Rigidbody selectedTarget = null;

    private bool isActive = true;
    private float targetRefreshTimer = 0f;

    private void Awake()
    {
      targetFinder = GetComponent<TargetFinder>();
      turretHub = GetComponentInChildren<TurretHub>();
      gun = GetComponentInChildren<GunController>();
      fireZone = gun.FireZone;
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

        bool targetIsInRotationLimit = CheckTargetIsWithinRotationLimits(target);

        if (targetIsInRotationLimit) targetsInRotationLimit.Add(target);
      }
    }

    private bool CheckTargetIsWithinRotationLimits( Rigidbody target )
    {
      return CheckTargetIsWithinRotationLimits(target, out float y, out float x);
    }

    private bool CheckTargetIsWithinRotationLimits( Rigidbody target, out float horizontalAngleDifference, out float verticleAngleDifference )
    {
      bool targetIsInRotationLimit = true;

      verticleAngleDifference = 0f;

      horizontalAngleDifference = GetTurretAngleDifferenceFromTarget(target);

      // Check if turret can rotate horizontally (y-axis) to face the target
      if (turretHub.RotationLimits.HasYLimits)
      {
        // Check if turret can rotate by this much
        float targetAngle = turretHub.TurretRotationValue + horizontalAngleDifference;
        Debug.Log(targetAngle);
        targetIsInRotationLimit = targetAngle >= turretHub.RotationLimits.MinYRot && targetAngle <= turretHub.RotationLimits.MaxYRot;
      }

      // Check if gun can rotate vertically (x-axis) to face the target
      if (targetIsInRotationLimit && turretHub.RotationLimits.HasXLimits)
      {
        float verticalAngleDifference = GetGunAngleDifferenceFromTarget(target, horizontalAngleDifference);

        // Check if turret can rotate by this much
        float targetAngle = turretHub.GunRotationValue + verticalAngleDifference;
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
      Vector3 expectedPosition = closestPoint + Vector3.Normalize(gun.FireZone.forward) * distToExpectedPosition;

      // Local position of the target to the turret
      Vector3 targetLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(target.position);
      // Local position of where the projectile leaves the gun to the turret
      Vector3 expectedLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(expectedPosition);

      // Remove y-axis
      Vector3 from = targetLocalPositionFromTurret;
      from.y = 0f;
      Vector3 to = expectedLocalPositionFromTurret;
      to.y = 0f;

      // Since we are not using y-axis, axis just points downward locally
      float angleBetween = Vector3.SignedAngle(from, to, Vector3.down);

      return angleBetween;
    }

    private float GetGunAngleDifferenceFromTarget( Rigidbody target, float turretRotation )
    {
      // What happens here is
      // 1. I need to horizontally rotate the gun direction to face the target based on the turret's rotation
      // 2. I need to correct the offset of the gun's fire zone to it's pivot point for the target's local position. This offset is also rotated.
      // This offset is required because the gun's direction will be calculated from the gun's pivot zone rather than it's fire zone.
      // 3. I find the angle between the rotated gun direction and the target's local position to the gun after correcting for offset.
      
      // Local position of the target to the gun
      Vector3 targetLocalPositionFromGun = gun.transform.InverseTransformPoint(target.position);

      // Find the rotation vector to rotate the gun direction and offset, this is rotated by the hub's base rotation taken from the slot's rotation.
      Vector3 rotationAngles = transform.rotation * new Vector3(0f, turretRotation, 0f);

      // World position of the gun's fire zone
      Vector3 fireZonePoint = gun.FireZone.position;
      // World position of the gun's fire direction from the gun's pivot point
      Vector3 fireDirectionFromGun = gun.transform.position + gun.FireZone.forward;

      // World position of the gun's fire zone point horizontally rotated to face the target
      Vector3 rotatedFireZonePoint = MathHelper.RotatePointAroundPivot(fireZonePoint, gun.transform.position, rotationAngles);
      // World position of the gun's fire direction from the gun's pivot point horizontally rotated to face the target
      Vector3 rotatedFireDirectionFromGun = MathHelper.RotatePointAroundPivot(fireDirectionFromGun, gun.transform.position, rotationAngles);

      // Offset between the fire zone and the gun's pivot point
      Vector3 expectedOffsetFromGun = gun.transform.InverseTransformPoint(rotatedFireZonePoint);
      // Gun's fire direction locally from the gun's pivot point
      Vector3 expectedFireDirectionFromGun = gun.transform.InverseTransformPoint(rotatedFireDirectionFromGun);

      // The target's local position from the gun corrected for offset
      Vector3 targetPositionOffsetFromGun = targetLocalPositionFromGun/* - expectedOffsetFromGun*/;
      //targetPositionOffsetFromGun.y -= expectedOffsetFromGun.y;

      //expectedFireDirectionFromGun.x = targetPositionOffsetFromGun.x;
      //expectedFireDirectionFromGun.z = targetPositionOffsetFromGun.z;

      Vector3 axisPointFromGun = gun.transform.position + gun.FireZone.right;
      Vector3 rotatedAxis = MathHelper.RotatePointAroundPivot(axisPointFromGun, gun.transform.position, rotationAngles);

      // Calculate the angle between
      float angle = Vector3.SignedAngle(expectedFireDirectionFromGun, targetPositionOffsetFromGun, rotatedAxis);

      Debug.Log($"{expectedFireDirectionFromGun.ToString("F2")} {targetPositionOffsetFromGun.ToString("F2")} {expectedOffsetFromGun.ToString("F2")} {angle}");



      return angle;
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
      if (CheckTargetIsWithinRotationLimits(selectedTarget))
      {
        float horizontalAngleDifference = GetTurretAngleDifferenceFromTarget(selectedTarget);
        turretHub.RotateTurretByValue(horizontalAngleDifference);
        //turretHub.RotateGunByValue(GetGunAngleDifferenceFromTarget(selectedTarget, horizontalAngleDifference));
      }
    }
  }
}
