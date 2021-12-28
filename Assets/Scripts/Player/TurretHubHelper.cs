using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  public static class TurretHubHelper
  {
    /// <summary>
    /// Checks whether the turret can rotate to face the inputted target.
    /// </summary>
    /// <param name="target">
    /// Target to check for.
    /// </param>
    /// <returns>
    /// Whether the turret can rotate to face the inputted target.
    /// </returns>
    public static bool CheckTargetInRotationRange( this TurretHub turretHub, Rigidbody target )
    {
      return turretHub.CheckTargetInRotationRange(target, out _, out _);
    }

    /// <summary>
    /// Checks whether the turret can rotate to face the inputted target.
    /// </summary>
    /// <param name="target">
    /// Target to check for.
    /// </param>
    /// <param name="horizontalAngleDistance">
    /// The y-axis angular distance the turret has to rotate to face the target.
    /// </param>
    /// <param name="verticalAngleDistance">
    /// The x-axis angular distance the turret has to rotate to face the target.
    /// </param>
    /// <returns> Whether the turret can rotate to face the inputted target. </returns>
    public static bool CheckTargetInRotationRange( this TurretHub turretHub, Rigidbody target, out float horizontalAngleDistance, out float verticalAngleDistance )
    {
      horizontalAngleDistance = 0f;
      verticalAngleDistance = 0f;

      bool targetIsInRotationLimit = true;

      horizontalAngleDistance = turretHub.GetTurretAngleDifferenceFromTarget(target);

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
        verticalAngleDistance = turretHub.GetGunAngleDifferenceFromTarget(target, horizontalAngleDistance);

        // Check if turret can rotate by this much
        float targetAngle = turretHub.GunRotationValue + verticalAngleDistance;
        targetIsInRotationLimit = targetAngle >= turretHub.RotationLimits.MinXRot && targetAngle <= turretHub.RotationLimits.MaxXRot;
      }

      return targetIsInRotationLimit;
    }

    /// <summary>
    /// Finds the angular distance between the turret's y-axis and the target. This is typically the turret's rotation (left and right).
    /// </summary>
    /// <param name="target">
    /// The target to check against.
    /// </param>
    /// <returns>
    /// The y-axis angular distance the turret has to rotate to face the target.
    /// </returns>
    public static float GetTurretAngleDifferenceFromTarget( this TurretHub turretHub, Rigidbody target )
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
      Vector3 firePosition = turretHub.FireZone.position;
      Vector3 fireDirectionEnd = firePosition + turretHub.FireZone.forward;

      // Projected vector of offset direction of fire zone from turret onto fire direction
      Vector3 projectedVector = Vector3.Project(turretPosition - firePosition, fireDirectionEnd - firePosition);
      // Closest point from the turret to the fire direction
      Vector3 closestPoint = firePosition + projectedVector;

      // Pythogras theorem to get distance to expected position
      float SqrMagToTarget = Vector3.SqrMagnitude(turretPosition - target.position);
      float SqrMagToClosestPoint = Vector3.SqrMagnitude(turretPosition - closestPoint);
      float distToExpectedPosition = Mathf.Sqrt(SqrMagToTarget - SqrMagToClosestPoint);

      // Expected position is from the closest point moved forward by the fire direction 
      Vector3 expectedTargetPosition = closestPoint + Vector3.Normalize(turretHub.FireZone.forward) * distToExpectedPosition;

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

    /// <summary>
    /// Finds the angular distance between the gun's x-axis and the target. This is typically the gun's rotation (up and down).
    /// GetTurretAngleDifferenceFromTarget has to be called first and its return value passed to this method.
    /// </summary>
    /// <param name="target">
    /// The target to check against.
    /// </param>
    /// <param name="horizontalRotation">
    /// The y-axis angular distance the turret has to rotate to face the target.
    /// </param>
    /// <returns>
    /// The x-axis angular distance the gun has to rotate to face the target.
    /// </returns>
    public static float GetGunAngleDifferenceFromTarget( this TurretHub turretHub, Rigidbody target, float horizontalRotation )
    {
      // Assume the turret can rotate horizontally to face the target.

      // Find the rotation vector to rotate the target so that it lines up with the gun on its local vertical plane.
      // This is rotated by the hub's base rotation (transform.rotation) taken from the slot's rotation.
      Vector3 rotationAngles = turretHub.transform.rotation * new Vector3(0f, -horizontalRotation, 0f);
      // Rotate the target about the pivot point so that it is on the same local vertical plane as the turret
      Vector3 horizontallyRotatedTargetPosition = MathHelper.RotatePointAroundPivot(target.position, turretHub.Turret.position, rotationAngles);

      // Find the closest point from the turret to the fire direction ray
      // https://stackoverflow.com/questions/51905268/how-to-find-closest-point-on-line
      Vector3 gunPosition = turretHub.Gun.position;
      Vector3 firePosition = turretHub.FireZone.position;
      Vector3 fireDirectionEnd = firePosition + turretHub.FireZone.forward;

      // Projected vector of offset direction of fire zone from turret onto fire direction
      Vector3 projectedVector = Vector3.Project(gunPosition - firePosition, fireDirectionEnd - firePosition);
      // Closest point from the turret to the fire direction
      Vector3 closestPoint = firePosition + projectedVector;

      // Pythogras theorem to get distance to expected position
      float SqrMagToTarget = Vector3.SqrMagnitude(gunPosition - horizontallyRotatedTargetPosition);
      float SqrMagToClosestPoint = Vector3.SqrMagnitude(gunPosition - closestPoint);
      float distToExpectedPosition = Mathf.Sqrt(SqrMagToTarget - SqrMagToClosestPoint);

      // Expected position is from the closest point moved forward by the fire direction 
      Vector3 expectedTargetPosition = closestPoint + Vector3.Normalize(turretHub.FireZone.forward) * distToExpectedPosition;

      // Local position of the target to the turret
      Vector3 targetLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(horizontallyRotatedTargetPosition);
      // Local position of where the projectile leaves the gun to the turret
      Vector3 expectedLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(expectedTargetPosition);

      // Since we are not using x-axis, axis just points leftwards locally
      float angularDistance = Vector3.SignedAngle(targetLocalPositionFromTurret, expectedLocalPositionFromTurret, Vector3.left);

      return angularDistance;
    }
  }
}
