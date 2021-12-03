using System;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  public class ScheduledTimer
  {
    public Action schedulerTick;
    public float tickLength;
    private float timer;

    //Time represents the time between ticks
    public ref Action Initialise(float time)
    {
      tickLength = time;
      timer = tickLength;
      return ref schedulerTick;
    }

    // Called in FixedUpdate by Scheduler because otherwise the timer will still update when the game is paused
    public void Run()
    {
      timer -= Time.deltaTime;

      if (timer > 0) return;
      timer = tickLength;
      schedulerTick?.Invoke();
    }
  }
}