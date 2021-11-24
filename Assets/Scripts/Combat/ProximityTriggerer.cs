using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  [RequireComponent(typeof(TargetFinder))]
  public class ProximityTriggerer : Triggerer<ProximityTriggerStats>
  {
    [Header("Proximity Values")]
    [SerializeField, Range(0f, 5f)] private float checkingInterval = 0.5f;

    private TargetFinder targetFinder;
    private SphereCollider proximityCollider;

    private float checkingCountdown = 0f;

    private void Awake()
    {
      targetFinder = GetComponent<TargetFinder>();
      proximityCollider = GetComponent<SphereCollider>();

      SetProximityColliderRadius();
      TriggerStats.TriggerRadius.OnStatUpgrade += SetProximityColliderRadius;
    }

    private void Update()
    {
      checkingCountdown -= Time.deltaTime;
      if (checkingCountdown <= 0f) CheckTriggerCondition();
    }

    /// <summary>
    /// Checks if there are the required amount of objects in proximity and triggers if so.
    /// </summary>
    private void CheckTriggerCondition()
    {
      int validTargetsInProximity = targetFinder.TargetsInRange.Count;

      if (validTargetsInProximity >= TriggerStats.NumberToTrigger.IntValue) CommenceTrigger();

      checkingCountdown = checkingInterval;
    }

    /// <summary>
    /// Adjusts the sphere collider trigger based on the proximity radius.
    /// </summary>
    private void SetProximityColliderRadius()
    {
      proximityCollider.radius = TriggerStats.TriggerRadius.Value;
    }
  }
}
