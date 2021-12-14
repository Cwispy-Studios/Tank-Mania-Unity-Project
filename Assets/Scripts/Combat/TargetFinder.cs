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

      // Object must have a rigidbody
      if (rb == null) return;
      // Object must not already be found by TargetFinder
      if (targetsInRange.Contains(rb)) return;

      Damageable damageable = rb.GetComponent<Damageable>();

      // Object must be damageable to be targetable
      if (damageable == null) return;

      if (targetPreference.IsSoftMatchWith(damageable.UnitProperties))
      {
        damageable.OnObjectDie += RemoveDeadObject;
        targetsInRange.Add(rb);
      }
    }

    private void OnTriggerExit( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      if (targetsInRange.Contains(rb))
      {
        Damageable damageable = rb.GetComponent<Damageable>();
        damageable.OnObjectDie -= RemoveDeadObject;

        targetsInRange.Remove(rb);
      }
    }

    private void RemoveDeadObject( Damageable disabledObject )
    {
      Rigidbody rb = disabledObject.GetComponent<Rigidbody>();

      if (targetsInRange.Contains(rb))
      {
        disabledObject.OnObjectDie -= RemoveDeadObject; ;
        targetsInRange.Remove(rb);
      }
    }
  }
}
