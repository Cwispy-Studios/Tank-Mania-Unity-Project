using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public abstract class StatsGroup : ScriptableObject
  {
    private void OnEnable() 
    {
      SubscribeStats();
    }

    private void OnDisable()
    {
      UnsubscribeStats();
    }

    public abstract void SubscribeStats();
    public abstract void UnsubscribeStats();
  }
}
