using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public class TimerTriggerer : Triggerer<TimerTriggerStats>
  {
    private float timer;

    private void Awake()
    {
      ResetTimer();
      TriggerStats.TimeToTrigger.OnStatUpgrade += ResetTimer;
    }

    private void Update()
    {
      timer -= Time.deltaTime;

      if (timer <= 0f) CommenceTrigger();
    }

    public override void Trigger()
    {
      OnTriggerEvent?.Invoke();
      timer = TriggerStats.TimeToTrigger.Value;
    }

    private void ResetTimer()
    {
      timer = TriggerStats.TimeToTrigger.Value;
    }
  }
}
