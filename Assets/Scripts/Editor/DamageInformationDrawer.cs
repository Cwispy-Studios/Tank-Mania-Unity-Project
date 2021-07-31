using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Editor
{
  using Combat;

  [CustomPropertyDrawer(typeof(DamageInformation))]
  public class DamageInformationDrawer : PropertyDrawer
  {
    private const int Padding = 4;
    private const float SingleLineHeightWithPadding = 16 + Padding;

    private int additionalLineCount = 0;

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      additionalLineCount = 0;

      EditorGUI.BeginProperty(position, label, property);

      // Default GUIStyle
      GUIStyle guiStyle = new GUIStyle(GUI.skin.label);
      guiStyle.padding.left = 1;
      guiStyle.padding.right = 1;

      // Each property's rect should be single line from here on
      position.height = EditorGUIUtility.singleLineHeight;

      // Retrieve the directDamageProperty
      SerializedProperty directDamageProperty = property.FindPropertyRelative("directDamage");
      // Create the slider for the direct damage
      EditorGUI.Slider(position, directDamageProperty, 0f, 10000f);

      // Move down 1 line
      position.y += SingleLineHeightWithPadding;

      // Retrieve the splash damage information from the damage information, and get if it is activated
      SerializedProperty splashDamageProperty = property.FindPropertyRelative(nameof(SplashDamageInformation));
      SerializedProperty hasSplashDamageProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.HasSplashDamage));

      float originalX = position.x;
      float originalWidth = position.width;

      position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(splashDamageProperty.displayName), guiStyle);
      hasSplashDamageProperty.boolValue = EditorGUI.Toggle(position, hasSplashDamageProperty.boolValue);

      // Reset rect back to left side
      position.x = originalX;
      position.width = originalWidth;

      int indent = EditorGUI.indentLevel;

      if (hasSplashDamageProperty.boolValue)
      {
        ++EditorGUI.indentLevel;

        additionalLineCount += 2;

        // Move down 1 line
        position.y += SingleLineHeightWithPadding;

        // Retrieve the splash damage information
        SerializedProperty splashRadiusProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.SplashRadius));
        SerializedProperty splashDamagePercentageProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.SplashDamagePercentage));

        EditorGUI.Slider(position, splashRadiusProperty, 0f, 5f);

        // Move down 1 line
        position.y += SingleLineHeightWithPadding;

        EditorGUI.Slider(position, splashDamagePercentageProperty, 0f, 1f);
      }

      // Move down 2 lines
      position.y += SingleLineHeightWithPadding * 1.5f;

      // Check if the splash damage rolloff is activated
      SerializedProperty hasSplashDamageRolloffProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.HasSplashDamageRolloff));

      // Create splash damage rolloff header
      position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Splash Damage Rolloff"), guiStyle);
      // Correct toggle position for indentation level (15 each indentation)
      position.x -= 15f;
      hasSplashDamageRolloffProperty.boolValue = EditorGUI.Toggle(position, hasSplashDamageRolloffProperty.boolValue);

      // Reset rect back to left side
      position.x = originalX;
      position.width = originalWidth;

      if (hasSplashDamageRolloffProperty.boolValue)
      {
        ++EditorGUI.indentLevel;

        additionalLineCount += 4;

        // Move down 1 line
        position.y += SingleLineHeightWithPadding;

        // Retrieve the splash damage rolloff information
        SerializedProperty minRadiusPercentageProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.MinRadiusPercentageRolloff));
        SerializedProperty maxRadiusPercentageProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.MaxRadiusPercentageRolloff));
        SerializedProperty minRadiusDamagePercentageProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.MinRadiusDamagePercentageRolloff));
        SerializedProperty maxRadiusDamagePercentageProperty = splashDamageProperty.FindPropertyRelative(nameof(SplashDamageInformation.MaxRadiusDamagePercentageRolloff));

        // Min Radius Percentage
        EditorGUI.Slider(position, minRadiusPercentageProperty, 0f, maxRadiusPercentageProperty.floatValue);

        // Move down 1 line
        position.y += SingleLineHeightWithPadding;

        // Max Radius Percentage
        EditorGUI.Slider(position, maxRadiusPercentageProperty, minRadiusPercentageProperty.floatValue, 1f);

        // Move down 1 line
        position.y += SingleLineHeightWithPadding;

        // Min Radius Damage Percentage
        EditorGUI.Slider(position, minRadiusDamagePercentageProperty, maxRadiusDamagePercentageProperty.floatValue, 1f);

        // Move down 1 line
        position.y += SingleLineHeightWithPadding;

        // Max Radius Damage Percentage
        EditorGUI.Slider(position, maxRadiusDamagePercentageProperty, 0f, minRadiusDamagePercentageProperty.floatValue);
      }

      EditorGUI.indentLevel = indent;

      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      int defaultLineCount = 3;
      int totalLineCount = defaultLineCount + additionalLineCount;

      return (base.GetPropertyHeight(property, label) + Padding / 2) * (totalLineCount + 0.5f);
    }
  }
}
