using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [CustomPropertyDrawer(typeof(StatUpgrader))]
  public class StatUpgraderPropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return base.GetPropertyHeight(property, label) * 2f + EditorGUIUtility.standardVerticalSpacing * 2f;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      SerializedProperty statUpgraded = property.FindPropertyRelative(nameof(statUpgraded));
      SerializedProperty statModifier = property.FindPropertyRelative(nameof(statModifier));

      position.height = EditorGUIUtility.singleLineHeight;
      position.y += EditorGUIUtility.standardVerticalSpacing;

      EditorGUI.BeginChangeCheck();

      statUpgraded.objectReferenceValue = 
        EditorGUI.ObjectField(position, "Upgrades:", statUpgraded.objectReferenceValue, typeof(Stat), false);

      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      EditorGUI.PropertyField(position, statModifier);

      if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }
  }
}
