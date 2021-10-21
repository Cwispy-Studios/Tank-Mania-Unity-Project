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

    private float testValuePlayerModifiers = 10f;
    private float testValueEnemyModifiers = 10f;

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

      // Unless customised, editing multiple objects just overwrites the values of the modifier arrays, so leave it as it is for now
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

      DrawModifierProperties(playerStatUpgraders, ref testValuePlayerModifiers);

      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(enemyStatUpgraders, new GUIContent("Enemy Upgrade Components"));

      DrawModifierProperties(enemyStatUpgraders, ref testValueEnemyModifiers);

      serializedObject.ApplyModifiedProperties();
    }

    private void DrawModifierProperties( SerializedProperty statUpgraders, ref float testValue )
    {
      EditorGUILayout.Space();

      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      EditorGUILayout.Space();

      testValue = EditorGUILayout.FloatField("Test Value:", testValue);

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
          modifiersList += $"\n   TV: {testValue} | <b>UV: {(testValue + additiveValue.floatValue) * (1f + multiplicativeValue.floatValue)}</b>";
        }
      }

      EditorGUILayout.TextArea(modifiersList, helpBoxStyle);
    }
  }
}
