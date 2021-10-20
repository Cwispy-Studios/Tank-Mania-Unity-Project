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
    private const float StatTypeButtonWidth = 7f;

    public const string UseIntPropertyName = "useInt";
    public const string BaseValuePropertyName = "baseValue";

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

    private bool objectAssigned;
    private bool showModifiers = false;
    private List<StatModifier> modifiersInList = new List<StatModifier>();

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      float height = base.GetPropertyHeight(property, label);

      objectAssigned = property.objectReferenceValue != null;

      if (objectAssigned)
      {
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      }

      if (showModifiers)
      {
        height += EditorGUI.GetPropertyHeight(modifiersList, true) + EditorGUIUtility.standardVerticalSpacing;
      }

      return height;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      position.height = EditorGUIUtility.singleLineHeight;

      // Make space for the stat type button, but only if object has already been assigned
      Rect prefixLabelRect = position;
      if (objectAssigned)
      {
        prefixLabelRect.x += StatTypeButtonWidth;
        prefixLabelRect.width = EditorGUIUtility.labelWidth - StatTypeButtonWidth;
      }
      EditorGUI.LabelField(prefixLabelRect, label);

      GUI.enabled = false;
      EditorGUI.PropertyField(position, property);
      GUI.enabled = true;

      // Do not need to show or initialise any values if stat is not assigned yet.
      if (!objectAssigned) return;

      SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
      serializedObject.Update();
      InitialiseVariables(serializedObject, label);
      SetStatType(serializedObject);

      //label.text += $" ({modifiersList.arraySize}):";

      EditorGUI.BeginChangeCheck();

      Rect buttonRect = EditorGUI.IndentedRect(position);
      buttonRect.width = 7f;

      SerializedProperty useInt = serializedObject.FindProperty("useInt");

      DrawStatTypeButton(buttonRect, useInt);

      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      DrawValueField(position, serializedObject, label);

      position.x += position.width + buttonMargin;
      position.width = SwapButtonWidth;

      // Disable multi-object editing for this property since it throws errors
      if (modifiersList.hasMultipleDifferentValues)
      {
        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

        return;
      }

      //// Draw the dropdown/foldout button to show/hide modifiers list
      //GUIContent content = showModifiers ? dropdownContent : foldoutContent;
      //content.tooltip = "Show/hide modifiers.\n\n" + modifiersTooltip;
      //if (GUI.Button(position, content, dropdownButtonStyle)) showModifiers = !showModifiers;

      //// Display the list of modifiers
      //if (showModifiers)
      //{
      //  position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      //  position.x = EditorGUIUtility.labelWidth + 18f;
      //  position.width = EditorGUIUtility.currentViewWidth - position.x - dropdownButtonStyle.margin.right;

      //  EditorGUI.PropertyField(position, modifiersList, new GUIContent("Modifiers", modifiersTooltip), true);
      //}

      if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }

    private void InitialiseVariables( SerializedObject serializedObject, GUIContent label )
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
        modifiersList = serializedObject.FindProperty(nameof(Stat.StatModifiers));
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

      if (serializedObject.isEditingMultipleObjects) return;

      modifiersInList.Clear();

      //for (int i = 0; i < modifiersList.arraySize; ++i)
      //{
      //  StatModifier statModifier = modifiersList.GetArrayElementAtIndex(i).objectReferenceValue as StatModifier;

      //  if (!modifiersInList.Contains(statModifier)) modifiersInList.Add(statModifier);
      //}

      //AddModifiersToTooltip(label);
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

      Color color = GUI.backgroundColor;
      GUI.backgroundColor = useInt.boolValue ? new Color(1f, 0.839f, 0.31f) : Color.cyan;

      GUI.Button(position, text);

      GUI.backgroundColor = color;

      GUI.enabled = true;
    }

    public virtual void SetStatType( SerializedObject serializedObject )
    {
      // Default use float
      serializedObject.FindProperty(UseIntPropertyName).boolValue = false;
    }

    public virtual void DrawValueField( Rect position, SerializedObject serializedObject, GUIContent label )
    {
      SerializedProperty baseValue = serializedObject.FindProperty(BaseValuePropertyName);

      bool useInt = serializedObject.FindProperty(UseIntPropertyName).boolValue;

      if (useInt)
        baseValue.floatValue = EditorGUI.IntField(position, label, (int)baseValue.floatValue);

      else
        baseValue.floatValue = EditorGUI.FloatField(position, label, baseValue.floatValue);
    }
  }
}
