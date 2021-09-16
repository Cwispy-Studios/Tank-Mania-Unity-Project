using UnityEngine;

// https://github.com/roboryantron/Unite2017/blob/59186d60af2cf1f5faf69cd45601607531ba260b/Assets/Code/Variables/FloatVariable.cs

namespace CwispyStudios.TankMania
{
  [CreateAssetMenu]
  public class FloatVariable : ScriptableObject
  {
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public float Value;

    public FloatVariable() { }

    public FloatVariable( float value )
    {
      Value = value;
    }
  }
}
