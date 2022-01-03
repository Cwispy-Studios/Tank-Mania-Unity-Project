// https://github.com/roboryantron/Unite2017/blob/59186d60af2cf1f5faf69cd45601607531ba260b/Assets/Code/Variables/FloatReference.cs

using System;

namespace CwispyStudios.TankMania
{
  [Serializable]
  public class FloatReference
  {
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public FloatReference() { }

    public FloatReference( float value ) 
    {
      UseConstant = true;
      ConstantValue = value;
    }

    public float Value
    {
      get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator float( FloatReference reference )
    {
      return reference.Value;
    }
  }
}
