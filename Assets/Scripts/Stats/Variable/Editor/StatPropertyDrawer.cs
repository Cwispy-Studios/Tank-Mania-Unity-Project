using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [CustomPropertyDrawer(typeof(Stat), true)]
  public class StatPropertyDrawer : PropertyDrawer
  {
    private const float SwapButtonWidth = 15f;

    private readonly string[] popupOptions = { "Float", "Int" };

    // Cache the style of the dropdown button
    private GUIStyle dropdownButtonStyle;
    // Cache the style of the popup button
    private GUIStyle popupStyle;

    // Cache the graphics of dropdown and foldout buttons
    private GUIContent foldoutContent;
    private GUIContent dropdownContent;

    // Cache the property of the modifiers list
    private SerializedProperty modifiersList;

    // Cache the margin size of a normal button
    private float buttonMargin;
    private string modifiersTooltip;

    private bool showModifiers = false;
    private List<StatModifier> modifiersInList = new List<StatModifier>();

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

      InitialiseVariables(property, label);

      label.text += $" ({modifiersList.arraySize}):";

      EditorGUI.BeginChangeCheck();

      position.height = EditorGUIUtility.singleLineHeight;
      Rect buttonRect = EditorGUI.IndentedRect(position);
      buttonRect.width = 10f;

      SerializedProperty useInt = property.FindPropertyRelative("useInt");

      DrawStatTypeButton(buttonRect, useInt);

      position.x += buttonRect.width;

      // Give space for the swap button after the property field
      position.width -= SwapButtonWidth + buttonMargin + buttonRect.width;

      SerializedProperty baseValue = property.FindPropertyRelative("baseValue");

      DrawValueField(position, baseValue, useInt, label);

      position.x += position.width + buttonMargin;
      position.width = SwapButtonWidth;

      // Disable multi-object editing for this property since it throws errors
      if (modifiersList.hasMultipleDifferentValues)
      {
        if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

        return;
      }

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

        EditorGUI.PropertyField(position, modifiersList, new GUIContent("Modifiers", modifiersTooltip), true);
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
      
      if (popupStyle == null)
      {
        popupStyle = GUI.skin.GetStyle("PaneOptions");
        popupStyle.imagePosition = ImagePosition.ImageOnly;
      }

      if (modifiersList == null)
      {
        modifiersList = property.FindPropertyRelative(nameof(Stat.StatModifiers));
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

      if (property.hasMultipleDifferentValues) return;
     
      modifiersInList.Clear();

      for (int i = 0; i < modifiersList.arraySize; ++i)
      {
        StatModifier statModifier = modifiersList.GetArrayElementAtIndex(i).objectReferenceValue as StatModifier;

        if (!modifiersInList.Contains(statModifier)) modifiersInList.Add(statModifier);
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

    private void DrawStatTypeButton( Rect position, SerializedProperty useInt )
    {
      GUI.enabled = false;

      string text = useInt.boolValue ? "i" : "f";
      GUI.Button(position, text);

      GUI.enabled = true;
    }

    public virtual void DrawValueField( Rect position, SerializedProperty baseValue, SerializedProperty useInt, GUIContent label )
    {
      //position = EditorGUI.PrefixLabel(position, label);

      if (useInt.boolValue)
        baseValue.floatValue = EditorGUI.IntField(position, label, (int)baseValue.floatValue);

      else
        baseValue.floatValue = EditorGUI.FloatField(position, label, baseValue.floatValue);
    }
  }
}
