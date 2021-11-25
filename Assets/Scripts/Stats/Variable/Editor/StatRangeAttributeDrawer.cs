using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(StatRangeAttribute))]
  public class StatRangeAttributeDrawer : StatPropertyDrawer
  {
    public override void SetStatType( SerializedObject serializedObject )
    {
      StatRangeAttribute range = attribute as StatRangeAttribute;

      serializedObject.FindProperty(UseIntPropertyName).boolValue = range.IsInt;
    }

    public override void DrawValueField( Rect position, SerializedObject serializedObject, GUIContent label )
    {
      StatRangeAttribute range = attribute as StatRangeAttribute;
      bool isInt = range.IsInt;

      SerializedProperty baseValue = serializedObject.FindProperty(BaseValuePropertyName);

      if (isInt)
        baseValue.floatValue = EditorGUI.IntSlider(position, label, (int)baseValue.floatValue, (int)range.MinValue, (int)range.MaxValue);

      else
        baseValue.floatValue = EditorGUI.Slider(position, label, baseValue.floatValue, range.MinValue, range.MaxValue);
    }
  }
}
