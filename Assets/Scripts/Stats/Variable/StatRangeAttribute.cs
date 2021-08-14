using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public class StatRangeAttribute : PropertyAttribute
  {
    public int minInt, maxInt;
    public float minFloat, maxFloat;

    public StatRangeAttribute( int min, int max )
    {
      minInt = min;
      maxInt = max;
    }

    public StatRangeAttribute( float min, float max )
    {
      minFloat = min;
      maxFloat = max;
    }
  }
}
