using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(StatRangeAttribute))]
  public class StatRangeAttributeDrawer : StatPropertyDrawer
  {
    public override void DrawValueField( Rect position, SerializedProperty baseValue, SerializedProperty useInt, GUIContent label )
    {
      StatRangeAttribute range = attribute as StatRangeAttribute;

      useInt.boolValue = range.UseInt;

      if (useInt.boolValue)
        baseValue.floatValue = EditorGUI.IntSlider(position, label, (int)baseValue.floatValue, (int)range.MinValue, (int)range.MaxValue);

      else
        baseValue.floatValue = EditorGUI.Slider(position, label, baseValue.floatValue, range.MinValue, range.MaxValue);
    }
  }
}
