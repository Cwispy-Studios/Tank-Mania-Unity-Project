using System;
using System.Collections.Generic;

namespace CwispyStudios.TankMania.Enemy
{
  public class Scheduler : Singleton<Scheduler>
  {
    private List<ScheduledTimer> activeTimers = new List<ScheduledTimer>();

    // If set to true, the value field is interpreted as the amount of times the clock ticks per second.
    // Otherwise, the value represents the amount of seconds between ticks
    public ref Action GetTimerTimesPerSecond(float time)
    {
      return ref GetTimer(1f / time);
    }

    //Time represents the time between ticks
    public ref Action GetTimer(float time)
    {
      // At initialization, check whether any schedulers exist with the same tickLength
      // If so return that timer's action
      foreach (var timer in activeTimers)
      {
        if (timer.tickLength != time) continue;
        
        return ref timer.schedulerTick;
      }

      // Else create a new timer
      ScheduledTimer newTimer = new ScheduledTimer();
      activeTimers.Add(newTimer);
      
      return ref newTimer.Initialise(time);
    }

    private void FixedUpdate()
    {
      foreach (var timer in activeTimers)
      {
        timer.Run();
      }
    }
  }
}