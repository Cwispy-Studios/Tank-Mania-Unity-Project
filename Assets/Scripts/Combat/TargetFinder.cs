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

    private void OnDisable()
    {
      targetsInRange.Clear();
    }

    private void OnTriggerEnter( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      // First check if object has rigidbody and is not already a known target
      if (rb == null || targetsInRange.Contains(rb)) return;

      // Then check if the target has a Damageable component
      Damageable damageable = rb.GetComponent<Damageable>();

      if (damageable != null && targetPreference.IsSoftMatchWith(damageable.UnitProperties))
      {
        damageable.OnObjectDisabled += RemoveDisabledObject;
        targetsInRange.Add(rb);
      }
    }

    private void OnTriggerExit( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      if (targetsInRange.Contains(rb))
      {
        rb.GetComponent<Damageable>().OnObjectDisabled -= RemoveDisabledObject; ;
        targetsInRange.Remove(rb);
      }
    }

    private void RemoveDisabledObject( GameObject disabledObject )
    {
      Rigidbody rb = disabledObject.GetComponent<Rigidbody>();

      if (targetsInRange.Contains(rb))
      {
        rb.GetComponent<Damageable>().OnObjectDisabled -= RemoveDisabledObject; ;
        targetsInRange.Remove(rb);
      }
    }
  }
}
