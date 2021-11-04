using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  public class TargetFinder : MonoBehaviour
  {
    private List<Rigidbody> targetsInRange = new List<Rigidbody>();
    public IList<Rigidbody> TargetsInRange => targetsInRange.AsReadOnly();

    private void OnTriggerEnter( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      if (rb != null && !targetsInRange.Contains(rb)) targetsInRange.Add(rb);
    }

    private void OnTriggerExit( Collider other )
    {
      Rigidbody rb = other.attachedRigidbody;

      if (rb != null && targetsInRange.Contains(rb)) targetsInRange.Remove(rb);
    }
  }
}
