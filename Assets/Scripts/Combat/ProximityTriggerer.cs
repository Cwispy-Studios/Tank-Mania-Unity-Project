using UnityEngine;
using UnityEngine.Events;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public class ProximityTriggerer : Triggerer<ProximityTriggerStats>
  {
    [Header("Proximity Values")]
    [SerializeField] private LayerMask layersToCheck;
    [SerializeField, Range(0f, 5f)] private float checkingInterval = 0.5f;

    // Cache the results of overlapsphere
    private Collider[] results;
    private float checkingCountdown = 0f;

    private void Awake()
    {
      BuildArray();
      TriggerStats.NumberToTrigger.OnStatUpgrade += BuildArray;
    }

    private void Update()
    {
      checkingCountdown -= Time.deltaTime;
      if (checkingCountdown <= 0f) CheckForObjects();
    }

    private void CheckForObjects()
    {
      int numberColliders = Physics.OverlapSphereNonAlloc(
        transform.position, TriggerStats.TriggerRadius.Value, results, layersToCheck, QueryTriggerInteraction.Ignore);

      if (numberColliders >= TriggerStats.NumberToTrigger.IntValue)
      {
        CommenceTrigger();
      }

      checkingCountdown = checkingInterval;
    }

    private void BuildArray()
    {
      results = new Collider[TriggerStats.NumberToTrigger.IntValue];
    }
  }
}
