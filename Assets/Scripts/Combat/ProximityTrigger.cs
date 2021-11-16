using UnityEngine;
using UnityEngine.Events;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public class ProximityTrigger : MonoBehaviour
  {
    [Header("Proximity Values")]
    [SerializeField] private LayerMask layersToCheck;
    [SerializeField] private ProximityTriggerStats proximityTriggerStats;
    [SerializeField, Range(0f, 5f)] private float checkingInterval = 0.5f;

    [SerializeField] public UnityEvent OnTriggerEvent;

    // Cache the results of overlapsphere
    private Collider[] results;
    private float checkingCountdown = 0f;

    private void Awake()
    {
      BuildArray();
      proximityTriggerStats.NumberToTrigger.OnStatUpgrade += BuildArray;
    }

    private void Update()
    {
      checkingCountdown -= Time.deltaTime;
      if (checkingCountdown <= 0f) CheckForObjects();
    }

    private void CheckForObjects()
    {
      int numberColliders = Physics.OverlapSphereNonAlloc(
        transform.position, proximityTriggerStats.TriggerRadius.Value, results, layersToCheck, QueryTriggerInteraction.Ignore);

      if (numberColliders >= proximityTriggerStats.NumberToTrigger.IntValue)
      {
        float delay = proximityTriggerStats.TriggerDelay.Value;

        if (delay > 0f) Invoke("Trigger", proximityTriggerStats.TriggerDelay.Value);
        else Trigger();
      }

      checkingCountdown = checkingInterval;
    }

    private void Trigger()
    {
      OnTriggerEvent?.Invoke();
    }

    private void BuildArray()
    {
      results = new Collider[proximityTriggerStats.NumberToTrigger.IntValue];
    }
  }
}
