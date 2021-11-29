using System;

using UnityEngine;

namespace CwispyStudios.TankMania
{
  public class PooledObject : MonoBehaviour
  {
    public event Action<GameObject> OnObjectDisabled;

    public virtual void OnDisable()
    {
      OnObjectDisabled?.Invoke(gameObject);
    }
  }
}
