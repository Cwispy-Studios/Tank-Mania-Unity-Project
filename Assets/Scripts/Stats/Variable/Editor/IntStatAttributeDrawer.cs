using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(IntStatAttribute))]
  public class IntStatAttributeDrawer : StatPropertyDrawer
  {
    public override void DrawValueField( Rect position, SerializedProperty baseValue, SerializedProperty useInt, GUIContent label )
    {
      useInt.boolValue = true;
      base.DrawValueField(position, baseValue, useInt, label);
    }
  }
}
