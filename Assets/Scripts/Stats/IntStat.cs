using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  public class IntStat : VariableStat
  {
    [SerializeField] private int baseValue;

    private int upgradedValue;

    private bool upgradedValueInitialised = false;

    public int Value
    {
      get
      {
        if (!upgradedValueInitialised)
        {
          upgradedValue = baseValue;
          upgradedValueInitialised = true;
        }

        return upgradedValue;
      }
    }

    /// <summary>
    /// Sets the default value in the inspector
    /// </summary>
    /// <param name="baseValue"></param>
    public IntStat( int baseValue )
    {
      this.baseValue = baseValue;
    }

    public override void RecalculateStat()
    {

    }
  }
}
