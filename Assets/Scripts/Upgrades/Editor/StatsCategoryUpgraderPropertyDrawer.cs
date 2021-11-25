using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [CustomPropertyDrawer(typeof(StatsCategoryUpgrader))]
  public class StatsCategoryUpgraderPropertyDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      return base.GetPropertyHeight(property, label) * 2f + EditorGUIUtility.standardVerticalSpacing * 2f;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      EditorGUI.BeginProperty(position, label, property);

      SerializedProperty statsCategoryUpgraded = property.FindPropertyRelative(nameof(statsCategoryUpgraded));
      SerializedProperty statModifier = property.FindPropertyRelative(nameof(statModifier));

      // Set the stat modifier's additive value to use an int/float slider based on all the stats' types
      if (statsCategoryUpgraded.objectReferenceValue != null)
      {
        // Get the serialized object to retrieve the list of stats in the upgrader
        SerializedObject statsCategorySerializedObject = new SerializedObject(statsCategoryUpgraded.objectReferenceValue);
        SerializedProperty statsInCategoryProperty = statsCategorySerializedObject.FindProperty("statsInCategory");

        bool useInt = true;

        // Loop through each stat
        for (int i = 0; i < statsInCategoryProperty.arraySize; ++i)
        {
          // Get the individual stat object
          Object statObject = statsInCategoryProperty.GetArrayElementAtIndex(i).objectReferenceValue;

          // Only valid if stat has been assigned a reference
          if (statObject != null)
          {
            // Get the serialized object to retrieve its use int value
            SerializedObject statSerializedObject = new SerializedObject(statObject);

            // If any stat is not an int variable, stat modifier additive slider uses float
            // If any stat is not an integer, the stat modifier will use a float slider
            if (!statSerializedObject.FindProperty("useInt").boolValue)
            {
              useInt = false;
              break;
            }
          }
        }

        statModifier.FindPropertyRelative("useInt").boolValue = useInt;
      }

      position.height = EditorGUIUtility.singleLineHeight;
      position.y += EditorGUIUtility.standardVerticalSpacing;

      EditorGUI.BeginChangeCheck();

      statsCategoryUpgraded.objectReferenceValue =
        EditorGUI.ObjectField(position, "Upgrades:", statsCategoryUpgraded.objectReferenceValue, typeof(StatsCategory), false);

      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      EditorGUI.PropertyField(position, statModifier);

      if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }
  }
}
