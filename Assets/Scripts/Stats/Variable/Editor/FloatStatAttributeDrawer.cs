using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(FloatStatAttribute))]
  public class FloatStatAttributeDrawer : StatPropertyDrawer
  {
    public override void DrawValueField( Rect position, SerializedProperty baseValue, SerializedProperty useInt, GUIContent label )
    {
      useInt.boolValue = false;
      base.DrawValueField(position, baseValue, useInt, label);
    }
  }
}
