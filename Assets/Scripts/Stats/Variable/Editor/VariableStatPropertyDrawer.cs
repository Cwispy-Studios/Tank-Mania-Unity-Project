using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [CustomPropertyDrawer(typeof(VariableStat), true)]
  public class VariableStatPropertyDrawer : PropertyDrawer
  {
    private const float SwapButtonWidth = 15f;

    // Cache the style of the dropdown button
    private GUIStyle dropdownButtonStyle;
    // Cache the graphics of dropdown and foldout buttons
    private GUIContent foldoutContent;
    private GUIContent dropdownContent;
    // Cache the property of the modifiers list
    private SerializedProperty modifiersList;
    // Cache the margin size of a normal button
    private float buttonMargin;
    private string modifiersTooltip;

    private bool showModifiers = false;

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      float height = base.GetPropertyHeight(property, label);

      if (showModifiers)
      {
        height += EditorGUI.GetPropertyHeight(modifiersList, true) + EditorGUIUtility.standardVerticalSpacing;
      }

      return height;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);
      label.text += ":";

      InitialiseVariables(property, label);

      EditorGUI.BeginChangeCheck();

      // Give space for the swap button after the property field
      position.width -= SwapButtonWidth + buttonMargin;
      position.height = EditorGUIUtility.singleLineHeight;

      DrawValueField(position, property.FindPropertyRelative("baseValue"), label);

      position.x += position.width + buttonMargin;
      position.width = SwapButtonWidth;

      // Draw the dropdown/foldout button to show/hide modifiers list
      GUIContent content = showModifiers ? dropdownContent : foldoutContent;
      content.tooltip = "Show/hide modifiers.\n\n" + modifiersTooltip;
      if (GUI.Button(position, content, dropdownButtonStyle)) showModifiers = !showModifiers;

      // Display the list of modifiers
      if (showModifiers)
      {
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        position.x = EditorGUIUtility.labelWidth + 18f;
        position.width = EditorGUIUtility.currentViewWidth - position.x - dropdownButtonStyle.margin.right;

        EditorGUI.BeginChangeCheck();

        EditorGUI.PropertyField(position, modifiersList, new GUIContent("Modifiers", modifiersTooltip), true);

        if (EditorGUI.EndChangeCheck())
        {
          for (int i = 0; i < modifiersList.arraySize; ++i)
          {
            StatModifier statModifier = modifiersList.GetArrayElementAtIndex(i).objectReferenceValue as StatModifier;

            if (statModifier)
              statModifier.AddStatModified($"{property.serializedObject.targetObject.name}: {property.displayName}");
          }
        }
      }

      if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }

    private void InitialiseVariables( SerializedProperty property, GUIContent label )
    {
      // Button for dropdown/foldout but without padding so image fills out the button space
      if (dropdownButtonStyle == null)
      {
        dropdownButtonStyle = GUI.skin.button;
        dropdownButtonStyle.padding = new RectOffset();
      }

      if (modifiersList == null)
      {
        modifiersList = property.FindPropertyRelative(nameof(VariableStat.StatModifiers));
      }

      if (buttonMargin < 1f)
      {
        buttonMargin = GUI.skin.button.margin.left;
      }

      // https://gist.github.com/MattRix/c1f7840ae2419d8eb2ec0695448d4321
      // https://assetstore.unity.com/packages/tools/utilities/unity-internal-icons-70496
      if (foldoutContent == null || dropdownContent == null)
      {
        foldoutContent = EditorGUIUtility.IconContent("d_IN_foldout_act");
        dropdownContent = EditorGUIUtility.IconContent("d_dropdown");
      }

      AddModifiersToTooltip(label);
    }

    private void AddModifiersToTooltip( GUIContent label )
    {
      // Add the list of modifiers to the tooltip of the stat
      int modifiersSize = modifiersList.arraySize;

      modifiersTooltip = $"Modifiers ({modifiersSize}):";

      if (modifiersSize == 0) modifiersTooltip += "\nNone";

      for (int i = 0; i < modifiersSize; ++i)
      {
        // Perform null check since object field can be null and unassigned
        Object modifierObject = modifiersList.GetArrayElementAtIndex(i).objectReferenceValue;

        if (modifierObject)
          modifiersTooltip += $"\n{modifierObject.name}";

        else modifiersTooltip += $"\nUNASSIGNED";
      }

      if (!string.IsNullOrEmpty(label.tooltip)) label.tooltip += "\n\n";

      label.tooltip += modifiersTooltip;
    }

    public virtual void DrawValueField( Rect position, SerializedProperty baseValue, GUIContent label )
    {
      EditorGUI.PropertyField(position, baseValue, label);
    }
  }
}
