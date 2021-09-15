using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Upgrades
{
  [CustomPropertyDrawer(typeof(StatModifierInstance))]
  public class StatModifierInstancePropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return base.GetPropertyHeight(property, label) * 2f + EditorGUIUtility.standardVerticalSpacing * 2f;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      SerializedProperty instanceName = property.FindPropertyRelative(nameof(instanceName));
      SerializedProperty statModifier = property.FindPropertyRelative(nameof(statModifier));

      position.height = EditorGUIUtility.singleLineHeight;
      position.y += EditorGUIUtility.standardVerticalSpacing;

      EditorGUI.BeginChangeCheck();

      EditorGUI.PropertyField(position, instanceName, new GUIContent());

      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      EditorGUI.PropertyField(position, statModifier);

      if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }
  }
}
