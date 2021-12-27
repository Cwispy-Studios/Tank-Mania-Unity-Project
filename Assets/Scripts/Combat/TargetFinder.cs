using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [RequireComponent(typeof(UnitTeam))]
  public class TargetsFinder : MonoBehaviour
  {
    [SerializeField] private TargetPreferences targetPreferences;

    private List<Rigidbody> targetsInRange = new List<Rigidbody>();
    public IList<Rigidbody> TargetsInRange => targetsInRange.AsReadOnly();

    private UnitTeam unitTeam;

    private void Awake()
    {
      unitTeam = GetComponent<UnitTeam>();
    }

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

      // Object must match the team criterias
      bool objectIsOpponent = unitTeam.OnOpposingTeam(rb.gameObject);
      bool matchesTeamCriterias = (targetPreferences.TargetsFriendlies && !objectIsOpponent) ||
        (targetPreferences.TargetsOpponents && objectIsOpponent);

      if (!matchesTeamCriterias) return;

      Damageable damageable = rb.GetComponent<Damageable>();
      
      // Object must be damageable to be targetable
      if (damageable == null) return;

      if (targetPreferences.TargetProperties.IsSoftMatchWith(damageable.UnitProperties))
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
