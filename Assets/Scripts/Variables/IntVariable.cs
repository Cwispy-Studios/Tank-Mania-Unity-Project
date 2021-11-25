using UnityEngine;

// https://github.com/roboryantron/Unite2017/blob/59186d60af2cf1f5faf69cd45601607531ba260b/Assets/Code/Variables/FloatVariable.cs

namespace CwispyStudios.TankMania
{
  [CreateAssetMenu(menuName = "Variables/Int Variable")]
  public class IntVariable : ScriptableObject
  {
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public int Value;

    public IntVariable() { }

    public IntVariable( int value )
    {
      Value = value;
    }
  }
}
