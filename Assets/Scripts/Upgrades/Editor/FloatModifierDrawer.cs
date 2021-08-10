using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CustomPropertyDrawer(typeof(FloatModifier))]
  public class FloatModifierDrawer : PropertyDrawer
  {
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      // Suffix with :, do not assign position since we want to draw below the prefix label
      //label.text += ":";
      EditorGUI.PrefixLabel(position, label);

      EditorGUI.BeginChangeCheck();

      // Get properties
      SerializedProperty additiveValue = property.FindPropertyRelative(nameof(additiveValue));
      SerializedProperty multiplicativeValue = property.FindPropertyRelative(nameof(multiplicativeValue));

      // Move down 1 line and reset the height
      position.y += EditorGUIUtility.singleLineHeight;
      position.height = EditorGUIUtility.singleLineHeight;

      // Additive label
      position.width = 10f;
      EditorGUI.LabelField(position, "+");

      // Skip past label, set width of the property field
      position.x += 10f;
      position.width = EditorGUIUtility.fieldWidth;

      // Property field of additive value
      EditorGUI.PropertyField(position, additiveValue, new GUIContent());

      // Skip past property field of additive value
      position.x += EditorGUIUtility.fieldWidth + 15f;

      // Multiplicative label percentage
      EditorGUI.LabelField(position, "+");

      // Skip past multiplicative label and set the width of the slider
      position.x += 10f;
      position.width = EditorGUIUtility.fieldWidth * 3f;

      // Display multiplicative value in percentages
      multiplicativeValue.floatValue = 
        EditorGUI.IntSlider(position, string.Empty, (int)(multiplicativeValue.floatValue * 100f), -100, 500) * 0.01f;

      // Skip past the slider
      position.x += EditorGUIUtility.fieldWidth * 3f;

      // Suffix with percentage symbol
      EditorGUI.LabelField(position, "%");

      if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return base.GetPropertyHeight(property, label) * 2f + EditorGUIUtility.standardVerticalSpacing;
    }
  }
}
