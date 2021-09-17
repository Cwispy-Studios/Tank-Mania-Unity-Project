using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public class StatRangeAttribute : PropertyAttribute
  {
    public bool UseInt;
    public float MinValue, MaxValue;

    public StatRangeAttribute( int min, int max )
    {
      UseInt = true;
      MinValue = min;
      MaxValue = max;
    }

    public StatRangeAttribute( float min, float max )
    {
      UseInt = false;
      MinValue = min;
      MaxValue = max;
    }
  }
}
