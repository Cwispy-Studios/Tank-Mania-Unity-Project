using UnityEngine;
using UnityEngine.Events;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public abstract class Triggerer<T> : MonoBehaviour where T : TriggerStats
  {
    [SerializeField] protected T TriggerStats;

    [SerializeField] protected UnityEvent OnTriggerEvent;

    protected void CommenceTrigger()
    {
      float delay = TriggerStats.TriggerDelay.Value;

      // Save on performance if no delay
      if (delay > 0f) Invoke("Trigger", TriggerStats.TriggerDelay.Value);
      else Trigger();
    }

    public virtual void Trigger()
    {
      OnTriggerEvent?.Invoke();
    }
  }
}
