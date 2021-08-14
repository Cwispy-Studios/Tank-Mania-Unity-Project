using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(StatRangeAttribute))]
  public class StatRangeAttributeDrawer : VariableStatPropertyDrawer
  {
    public override void DrawValueField( Rect position, SerializedProperty baseValue, GUIContent label )
    {
      StatRangeAttribute range = attribute as StatRangeAttribute;

      if (baseValue.propertyType == SerializedPropertyType.Integer)
        baseValue.intValue = EditorGUI.IntSlider(position, label, baseValue.intValue, range.minInt, range.maxInt);

      else if (baseValue.propertyType == SerializedPropertyType.Float)
        baseValue.floatValue = EditorGUI.Slider(position, label, baseValue.floatValue, range.minFloat, range.maxFloat);
    }
  }
}
