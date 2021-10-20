using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Upgrades
{
  [CustomPropertyDrawer(typeof(StatSubscription))]
  public class StatSubscriptionPropertyDrawer : PropertyDrawer
  {
    private const float SwapButtonWidth = 20f;
    private const float ButtonOffset = 25f;

    private float verticalSpace;

    // Cache the style of the dropdown button
    private GUIStyle dropdownButtonStyle;

    // Cache the graphics of dropdown and foldout buttons
    private GUIContent foldoutContent;
    private GUIContent dropdownContent;

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      // Base height with 2 lines
      float height = base.GetPropertyHeight(property, label) * 2f + EditorGUIUtility.standardVerticalSpacing;

      SerializedProperty selectingModifier = property.FindPropertyRelative(nameof(selectingModifier));

      if (selectingModifier.boolValue)
      {
        height += verticalSpace;

        SerializedProperty upgrade = property.FindPropertyRelative(nameof(upgrade));
        // Get the selected upgrade object
        Upgrade selectedUpgrade = upgrade.objectReferenceValue as Upgrade;

        if (selectedUpgrade)
        {
          // Get the total number of modifiers for GetPropertyHeight
          int numberOfModifiers = selectedUpgrade.PlayerStatModifiers.Count + selectedUpgrade.EnemyStatModifiers.Count;

          // + 2 for the labels
          int totalCount = numberOfModifiers + 2;
          height += EditorGUIUtility.singleLineHeight * totalCount + EditorGUIUtility.standardVerticalSpacing * totalCount;
        }
      }

      return height;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      InitialiseVariables();

      // These need to be local variables since an array of property drawers will reuse the same member variables
      SerializedProperty statModifier = property.FindPropertyRelative(nameof(StatSubscription.StatModifierInstance));
      SerializedProperty statModifierUpgrade = property.FindPropertyRelative(nameof(statModifierUpgrade));
      SerializedProperty upgrade = property.FindPropertyRelative(nameof(upgrade));
      SerializedProperty statModifierIndex = property.FindPropertyRelative(nameof(statModifierIndex));
      SerializedProperty selectingModifier = property.FindPropertyRelative(nameof(selectingModifier));

      position.height = EditorGUIUtility.singleLineHeight;

      // Show the chosen modifier
      string statModifierName = statModifier.FindPropertyRelative("instanceName").stringValue;
      statModifierName = string.IsNullOrEmpty(statModifierName) ? "NONE" : statModifierName;
      EditorGUI.LabelField(position, $"Chosen Modifier: {statModifierName}");

      position.y += verticalSpace;

      // Show the upgrade the modifier belongs to
      GUI.enabled = false;
      EditorGUI.PropertyField(position, statModifierUpgrade, new GUIContent("From:"));
      GUI.enabled = true;

      // Check for change in any property so it can be applied
      EditorGUI.BeginChangeCheck();

      float width = position.width;
      position.x -= ButtonOffset;
      position.width = SwapButtonWidth;

      // Draw the dropdown/foldout button to show/hide modifiers list
      GUIContent content = selectingModifier.boolValue ? dropdownContent : foldoutContent;
      content.tooltip = "Show/hide modifier selection.";
      if (GUI.Button(position, content, dropdownButtonStyle)) selectingModifier.boolValue = !selectingModifier.boolValue;

      position.x += ButtonOffset;
      position.width = width;

      if (selectingModifier.boolValue)
      {
        position.y += verticalSpace;

        EditorGUI.PropertyField(position, upgrade, new GUIContent("Upgrade"));

        bool hasUpgradeInstance = upgrade.objectReferenceValue != null;

        if (hasUpgradeInstance)
        {
          // Get the selected upgrade object
          Upgrade selectedUpgrade = upgrade.objectReferenceValue as Upgrade;

          position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

          EditorGUI.LabelField(position, "Player Modifiers:");

          for (int i = 0; i < selectedUpgrade.PlayerStatModifiers.Count; ++i)
          {
            string modifierName = selectedUpgrade.PlayerStatModifiers[i].InstanceName;

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (GUI.Button(position, modifierName))
            {
              //statModifier.managedReferenceValue = selectedUpgrade.PlayerStatModifiers[i];
              statModifierIndex.intValue = i;
              selectingModifier.boolValue = false;
            }
          }

          position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

          EditorGUI.LabelField(position, "Enemy Modifiers:");

          for (int i = 0; i < selectedUpgrade.EnemyStatModifiers.Count; ++i)
          {
            string modifierName = selectedUpgrade.EnemyStatModifiers[i].InstanceName;

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (GUI.Button(position, modifierName))
            {
              statModifierIndex.intValue = selectedUpgrade.PlayerStatModifiers.Count + i;
            }
          }
        }
      }

      if (EditorGUI.EndChangeCheck()) 
        property.serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }

    private void InitialiseVariables()
    {
      // Button for dropdown/foldout but without padding so image fills out the button space
      if (dropdownButtonStyle == null)
      {
        dropdownButtonStyle = GUI.skin.button;
        dropdownButtonStyle.padding = new RectOffset();
      }

      // https://gist.github.com/MattRix/c1f7840ae2419d8eb2ec0695448d4321
      // https://assetstore.unity.com/packages/tools/utilities/unity-internal-icons-70496
      if (foldoutContent == null || dropdownContent == null)
      {
        foldoutContent = EditorGUIUtility.IconContent("d_IN_foldout_act");
        dropdownContent = EditorGUIUtility.IconContent("d_dropdown");
      }

      if (verticalSpace == 0f)
      {
        verticalSpace = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      }
    }
  }
}
