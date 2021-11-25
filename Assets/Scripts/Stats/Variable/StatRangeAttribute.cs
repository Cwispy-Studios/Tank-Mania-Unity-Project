using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public class StatRangeAttribute : PropertyAttribute
  {
    public bool IsInt;
    public float MinValue, MaxValue;

    public StatRangeAttribute( int min, int max )
    {
      IsInt = true;
      MinValue = min;
      MaxValue = max;
    }

    public StatRangeAttribute( float min, float max )
    {
      IsInt = false;
      MinValue = min;
      MaxValue = max;
    }
  }
}
