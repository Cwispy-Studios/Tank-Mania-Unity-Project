using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Upgrades
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof(Upgrade), true)]
  public class UpgradeEditor : Editor
  {
    private SerializedProperty upgradeRarity;
    private SerializedProperty upgradeImage;
    private SerializedProperty upgradeName;
    private SerializedProperty upgradeDescription;
    private SerializedProperty playerStatUpgraders;
    private SerializedProperty enemyStatUpgraders;

    private void OnEnable()
    {
      upgradeRarity = serializedObject.FindProperty(nameof(Upgrade.UpgradeRarity));
      upgradeImage = serializedObject.FindProperty(nameof(Upgrade.UpgradeImage));
      upgradeName = serializedObject.FindProperty(nameof(Upgrade.UpgradeName));
      upgradeDescription = serializedObject.FindProperty(nameof(Upgrade.UpgradeDescription));
      playerStatUpgraders = serializedObject.FindProperty(nameof(Upgrade.PlayerStatUpgraders));
      enemyStatUpgraders = serializedObject.FindProperty(nameof(Upgrade.EnemyStatUpgraders));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      // Draws the disabled Script property at the top of every ScriptableObject
      EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
      EditorGUI.EndDisabledGroup();

      // Upgrade Rarity
      EditorGUILayout.PropertyField(upgradeRarity);

      if (serializedObject.isEditingMultipleObjects)
      {
        serializedObject.ApplyModifiedProperties();
        return;
      }

      // Upgrade Name, Image, Description
      EditorGUILayout.PropertyField(upgradeName);
      upgradeImage.objectReferenceValue = EditorGUILayout.ObjectField("Image", upgradeImage.objectReferenceValue, typeof(Sprite), false);
      EditorGUILayout.PropertyField(upgradeDescription);

      if (GUILayout.Button("Upgrade Name", GUILayout.ExpandWidth(false))) 
        upgradeDescription.stringValue += $"{upgradeName.stringValue} ";

      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(playerStatUpgraders, new GUIContent("Player Upgrade Components"));

      DrawModifierProperties(playerStatUpgraders);

      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(enemyStatUpgraders, new GUIContent("Enemy Upgrade Components"));

      DrawModifierProperties(enemyStatUpgraders);

      serializedObject.ApplyModifiedProperties();
    }

    private void DrawModifierProperties( SerializedProperty statUpgraders )
    {
      EditorGUILayout.Space();

      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      EditorGUILayout.Space();

      GUIStyle helpBoxStyle = GUI.skin.GetStyle("HelpBox");
      helpBoxStyle.richText = true;

      string modifiersList = $"<b>Modifiers List:</b>";

      for (int i = 0; i < statUpgraders.arraySize; ++i)
      {
        SerializedProperty statUpgraderProperty = statUpgraders.GetArrayElementAtIndex(i);

        SerializedProperty statUpgradedProperty = statUpgraderProperty.FindPropertyRelative("statUpgraded");
        SerializedProperty statModiferProperty = statUpgraderProperty.FindPropertyRelative("statModifier");

        SerializedProperty additiveValue = statModiferProperty.FindPropertyRelative(nameof(additiveValue));
        SerializedProperty multiplicativeValue = statModiferProperty.FindPropertyRelative(nameof(multiplicativeValue));

        if (statUpgradedProperty.objectReferenceValue != null)
        {
          modifiersList += $"\n• <i>{statUpgradedProperty.objectReferenceValue.name}</i>: "; 

          if (additiveValue.floatValue != 0f) modifiersList += $"+{additiveValue.floatValue.ToString("F2")} ";
          if (multiplicativeValue.floatValue != 0f) modifiersList += $"+{(multiplicativeValue.floatValue * 100f).ToString("F0")}%";
        }
      }

      EditorGUILayout.TextArea(modifiersList, helpBoxStyle);
    }
  }
}
