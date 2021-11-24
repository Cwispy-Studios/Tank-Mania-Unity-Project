using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  public class TargetFinder : MonoBehaviour
  {
    [Tooltip("Target must match the Team preference, and must contain ANY of the UnitType selected. If UnitType is 0, accepts anything.")]
    [SerializeField] private UnitProperties targetPreference;

    private List<Rigidbody> targetsInRange = new List<Rigidbody>();
    public IList<Rigidbody> TargetsInRange => targetsInRange.AsReadOnly();

    private void OnTriggerEnter( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      // First check if object has rigidbody and is not already a known target
      if (rb == null || targetsInRange.Contains(rb)) return;

      // Then check if the target has a Damageable component
      Damageable damageable = rb.GetComponent<Damageable>();

      if (damageable != null && TargetMatchesPreference(damageable))
      {
        targetsInRange.Add(rb);
      }
    }

    private void OnTriggerExit( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      if (targetsInRange.Contains(rb)) targetsInRange.Remove(rb);
    }

    private bool TargetMatchesPreference( Damageable damageable )
    {
      bool isPreferredTeam = targetPreference.Team == damageable.UnitProperties.Team;
      // If no stated UnitType preference, this will always be true, else check if target meets any selected preference
      bool isPreferredUnitType = targetPreference.UnitType == 0 ? true :
        (targetPreference.UnitType & damageable.UnitProperties.UnitType) != 0;

      return isPreferredTeam & isPreferredUnitType;
    }
  }
}
