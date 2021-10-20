using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CustomPropertyDrawer(typeof(StatModifier))]
  public class StatModifierEditor : PropertyDrawer
  {
    private SerializedProperty additiveValue;
    private SerializedProperty multiplicativeValue;

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      additiveValue = property.FindPropertyRelative(nameof(additiveValue));
      multiplicativeValue = property.FindPropertyRelative(nameof(multiplicativeValue));

      EditorGUI.BeginChangeCheck();

      // Width of the inspector
      float viewWidth = EditorGUIUtility.currentViewWidth - 58f;
      // Additive takes 1/4 width, multiplicative takes 3/4 width
      float additiveWidth = viewWidth * 0.25f;
      float multiplicativeWidth = viewWidth - additiveWidth;

      EditorGUIUtility.labelWidth = 9f;

      position.width = additiveWidth;

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
