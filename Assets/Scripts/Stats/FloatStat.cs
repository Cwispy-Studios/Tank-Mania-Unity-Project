using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [System.Serializable]
  public class FloatStat : VariableStat
  {
    [SerializeField] private float baseValue;

    private float upgradedValue;

    private bool upgradedValueInitialised = false;

    public float Value
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
    public FloatStat( float baseValue )
    {
      this.baseValue = baseValue;
    }

    public override void RecalculateStat()
    {

    }
  }
}
