using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Combat;

  public class TurretAIController : MonoBehaviour
  {
    private static float s_targetRefreshInterval = 0.5f;

    private TargetFinder targetFinder;
    private TurretHub turretHub;
    private GunController gun;
    private Transform fireZone;

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

        targetRefreshTimer = 0f;
      }
    }

    private void RefreshTargets()
    {
      List<Rigidbody> targetsInRange = new List<Rigidbody>(targetFinder.TargetsInRange);

      // Loop through every target in range...
      for (int i = 0; i < targetsInRange.Count; ++i)
      {
        Rigidbody target = targetsInRange[i];

        // Check if turret can rotate horizontally (y-axis) to face the target
        if (turretHub.TurretRotationLimits.HasYLimits)
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

          float horizontalAngleDifference = Vector3.SignedAngle(from, to, Vector3.up);

          turretHub.RotateTurretByValue(horizontalAngleDifference);
        }

        // Check if gun can rotate vertically (x-axis) to face the target
        if (turretHub.TurretRotationLimits.HasXLimits)
        {
          // Local position of the target to the gun
          Vector3 targetLocalPositionFromGun = gun.transform.InverseTransformPoint(target.position);
          // Local position of where the projectile leaves the gun to the gun
          Vector3 fireZoneLocalPositionFromGun = gun.transform.InverseTransformPoint(gun.FireZone.position);

          // Get angle difference between the fire zone's height to the target's height
          // This assumes turret is already facing the target, so x and z-axes will be the same as the target's
          Vector3 from = targetLocalPositionFromGun;
          from.y = fireZoneLocalPositionFromGun.y;
          Vector3 to = targetLocalPositionFromGun;

          float verticalAngleDifference = Vector3.SignedAngle(from, to, Vector3.forward);

          turretHub.RotateGunByValue(verticalAngleDifference);
        }
      }
    }
  }
}
