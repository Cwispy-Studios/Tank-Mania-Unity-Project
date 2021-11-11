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
      // Local position of the target to the turret
      Vector3 targetLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(target.position);
      // Local position of where the projectile leaves the gun to the turret
      Vector3 fireZoneLocalPositionFromTurret = turretHub.Turret.InverseTransformPoint(gun.FireZone.position);

      // If fire zone position is the same as the turret's position, use the local forward instead.
      if (fireZoneLocalPositionFromTurret.x == 0f && fireZoneLocalPositionFromTurret.z == 0f)
        fireZoneLocalPositionFromTurret = Vector3.forward;

      // Get angle difference between the fire zone and the target RELATIVE TO THE TURRET
      // Remove y-axis from both positions since we don't want to test that axis
      Vector3 from = fireZoneLocalPositionFromTurret;
      from.y = 0f;
      Vector3 to = targetLocalPositionFromTurret;
      to.y = 0f;

      return Vector3.SignedAngle(from, to, Vector3.up);
    }

    private float GetGunAngleDifferenceFromTarget( Rigidbody target, float turretRotation )
    {
      // Local position of the target to the gun
      Vector3 targetLocalPositionFromGun = gun.transform.InverseTransformPoint(target.position);
      // Local position of where the projectile leaves the gun to the gun
      Vector3 fireZoneLocalPositionFromGun = gun.transform.InverseTransformPoint(gun.FireZone.position);

      Vector3 rotationAngles = transform.rotation * new Vector3(0f, turretRotation, 0f);

      Vector3 fireZonePoint = gun.FireZone.position;
      Vector3 fireDirectionFromGun = gun.transform.position + gun.FireZone.forward;

      Vector3 rotatedOffset = MathHelper.RotatePointAroundPivot(fireZonePoint, gun.transform.position, rotationAngles);
      Vector3 rotatedFireDirectionFromGun = MathHelper.RotatePointAroundPivot(fireDirectionFromGun, gun.transform.position, rotationAngles);

      Vector3 expectedOffsetFromGun = gun.transform.InverseTransformPoint(rotatedOffset);
      Vector3 expectedFireDirectionFromGun = gun.transform.InverseTransformPoint(rotatedFireDirectionFromGun);

      Vector3 targetPositionOffsetFromGun = targetLocalPositionFromGun + expectedOffsetFromGun;

      float correctAngle = Vector3.Angle(expectedFireDirectionFromGun, targetPositionOffsetFromGun);

      Debug.Log($"{expectedOffsetFromGun} {expectedFireDirectionFromGun} {targetPositionOffsetFromGun} {correctAngle}");

      //Vector3 dir = gun.FireZone.forward - gun.transform.position;
      //dir = Quaternion.Euler(0f, turretRotation, 0f) * dir;
      //Vector3 point = gun.transform.position + dir;

      //Vector3 expectedFireZonePositionFromGun = gun.transform.InverseTransformPoint(point);

      // Get angle difference between the fire zone's height to the target's height
      // This assumes turret is already facing the target, so x and z-axes will be the same as the target's
      Vector3 from = targetLocalPositionFromGun;
      from.y = fireZoneLocalPositionFromGun.y;
      Vector3 to = targetLocalPositionFromGun;

      float angle = Vector3.Angle(from, to);

      if (to.y > from.y) angle *= -1f;

      //Debug.Log($"{angle} {from} {expectedFireZonePositionFromGun} {to}");

      return angle;

      //return Vector3.SignedAngle(from, to, axis);
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
        turretHub.RotateGunByValue(GetGunAngleDifferenceFromTarget(selectedTarget, horizontalAngleDifference));
      }
    }
  }
}
