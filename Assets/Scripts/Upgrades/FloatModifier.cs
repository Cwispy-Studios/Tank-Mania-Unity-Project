using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [System.Serializable]
  public class FloatModifier
  {
    [SerializeField] private float additiveValue = 0f;
    public float AddititiveValue => additiveValue;

    [SerializeField, Range(-1f, 10f)] private float multiplicativeValue = 0f;
    public float MultiplicativeValue => multiplicativeValue;

    public FloatModifier( float additive, float multiplicative )
    {
      additiveValue = additive;
      multiplicativeValue = multiplicative;
    }

    public static FloatModifier operator +( FloatModifier a, FloatModifier b ) =>
      new FloatModifier(a.additiveValue + b.additiveValue, a.multiplicativeValue + b.multiplicativeValue);
  }
}
