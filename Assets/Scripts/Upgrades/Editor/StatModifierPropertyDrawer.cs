using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CustomPropertyDrawer(typeof(StatModifier))]
  public class StatModifierPropertyDrawer : PropertyDrawer
  {
    private SerializedProperty additiveValue;
    private SerializedProperty multiplicativeValue;
    private SerializedProperty useInt;

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      additiveValue = property.FindPropertyRelative(nameof(additiveValue));
      multiplicativeValue = property.FindPropertyRelative(nameof(multiplicativeValue));
      useInt = property.FindPropertyRelative(nameof(useInt));

      EditorGUI.BeginChangeCheck();

      // Width of the inspector
      position = EditorGUI.IndentedRect(position);

      float additiveWidth = position.width * 0.25f;
      float multiplicativeWidth = position.width - additiveWidth;

      EditorGUIUtility.labelWidth = 9f;

      position.width = additiveWidth;

      if (useInt.boolValue)
        additiveValue.floatValue = EditorGUI.IntField(position, "+", Mathf.RoundToInt(additiveValue.floatValue));

      else
        additiveValue.floatValue = EditorGUI.FloatField(position, "+", additiveValue.floatValue);

      position.x += additiveWidth;
      position.width = multiplicativeWidth;

      int displayValue = Mathf.RoundToInt(multiplicativeValue.floatValue * 100f);
      multiplicativeValue.floatValue = EditorGUI.IntSlider(position, "+", displayValue, -100, 1000) * 0.01f;

      position.x += multiplicativeWidth - 15f;

      EditorGUI.LabelField(position, "%");
    }
  }
}
