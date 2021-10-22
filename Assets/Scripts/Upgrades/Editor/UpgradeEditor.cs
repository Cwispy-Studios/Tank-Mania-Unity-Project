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
    private SerializedProperty playerStatsCategoryUpgraders;
    private SerializedProperty enemyStatUpgraders;
    private SerializedProperty enemyStatsCategoryUpgraders;

    private float testValuePlayerModifiers = 10f;
    private float testValueEnemyModifiers = 10f;

    private void OnEnable()
    {
      upgradeRarity = serializedObject.FindProperty(nameof(Upgrade.UpgradeRarity));
      upgradeImage = serializedObject.FindProperty(nameof(Upgrade.UpgradeImage));
      upgradeName = serializedObject.FindProperty(nameof(Upgrade.UpgradeName));
      upgradeDescription = serializedObject.FindProperty(nameof(Upgrade.UpgradeDescription));
      playerStatUpgraders = serializedObject.FindProperty(nameof(Upgrade.PlayerStatUpgraders));
      playerStatsCategoryUpgraders = serializedObject.FindProperty(nameof(Upgrade.PlayerStatsCategoryUpgraders));
      enemyStatUpgraders = serializedObject.FindProperty(nameof(Upgrade.EnemyStatUpgraders));
      enemyStatsCategoryUpgraders = serializedObject.FindProperty(nameof(Upgrade.EnemyStatsCategoryUpgraders));
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
      
      // To add upgrade name to upgrade description
      if (GUILayout.Button("Upgrade Name", GUILayout.ExpandWidth(false))) 
        upgradeDescription.stringValue += $"{upgradeName.stringValue} ";

      EditorGUILayout.Space();

      // Seperator
      Rect lineRect = EditorGUILayout.GetControlRect(false, 2f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      // Player upgraders
      EditorGUILayout.PropertyField(playerStatUpgraders, new GUIContent("Stats"));
      EditorGUILayout.PropertyField(playerStatsCategoryUpgraders, new GUIContent("Stats Categories"));

      DrawModifierProperties(playerStatUpgraders, playerStatsCategoryUpgraders, ref testValuePlayerModifiers);

      EditorGUILayout.Space();

      // Seperator
      lineRect = EditorGUILayout.GetControlRect(false, 2f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      // Enemy upgraders
      EditorGUILayout.PropertyField(enemyStatUpgraders, new GUIContent("Stats"));
      EditorGUILayout.PropertyField(enemyStatsCategoryUpgraders, new GUIContent("Stats Categories"));

      DrawModifierProperties(enemyStatUpgraders, enemyStatsCategoryUpgraders, ref testValueEnemyModifiers);

      serializedObject.ApplyModifiedProperties();
    }

    private void DrawModifierProperties( SerializedProperty statUpgraders, SerializedProperty statsCategoryUpgraders, ref float testValue )
    {
      EditorGUILayout.Space();

      testValue = EditorGUILayout.FloatField("Test Value:", testValue);

      GUIStyle helpBoxStyle = GUI.skin.GetStyle("HelpBox");
      helpBoxStyle.richText = true;

      string modifiersList = $"<b>Modifiers List:</b>";

      // Fill modifiers list for stat upgraders, loop through each stat upgrader...
      for (int i = 0; i < statUpgraders.arraySize; ++i)
      {
        // Retrieve its property
        SerializedProperty statUpgraderProperty = statUpgraders.GetArrayElementAtIndex(i);

        // Retrieve the properties for the stat it upgrades and the stat modifier
        SerializedProperty statUpgradedProperty = statUpgraderProperty.FindPropertyRelative("statUpgraded");
        SerializedProperty statModiferProperty = statUpgraderProperty.FindPropertyRelative("statModifier");

        // Retrieve the properties for the additive and multiplicative values from the stat modifier
        SerializedProperty additiveValue = statModiferProperty.FindPropertyRelative(nameof(additiveValue));
        SerializedProperty multiplicativeValue = statModiferProperty.FindPropertyRelative(nameof(multiplicativeValue));

        FillModifiersList(statUpgradedProperty, additiveValue, multiplicativeValue, ref modifiersList, ref testValue);
      }

      // Fill modifiers list for stat category upgraders, loop through each stat category upgrader...
      for (int i = 0; i < statsCategoryUpgraders.arraySize; ++i)
      {
        // Retrieve its property
        SerializedProperty statsCategoryUpgraderProperty = statsCategoryUpgraders.GetArrayElementAtIndex(i);
        // Retrieve the stat category property from the upgrader
        SerializedProperty statsCategoryProperty = statsCategoryUpgraderProperty.FindPropertyRelative("statsCategoryUpgraded");

        // Check that a stat category reference has been assigned
        if (statsCategoryProperty.objectReferenceValue != null)
        {
          // Retrieve the serialized object of the stat category to get its list of stats
          SerializedObject statsCategorySerializedObject = new SerializedObject(statsCategoryProperty.objectReferenceValue);
          SerializedProperty statsInCategoryProperty = statsCategorySerializedObject.FindProperty("statsInCategory");

          // Retrieve the stat modifier of the upgrader
          SerializedProperty statModiferProperty = statsCategoryUpgraderProperty.FindPropertyRelative("statModifier");

          // .. and its values
          SerializedProperty additiveValue = statModiferProperty.FindPropertyRelative(nameof(additiveValue));
          SerializedProperty multiplicativeValue = statModiferProperty.FindPropertyRelative(nameof(multiplicativeValue));

          // Loop through each stat in the stat category
          for (int j = 0; j < statsInCategoryProperty.arraySize; ++j)
          {
            // Retrieve the stat property
            SerializedProperty statUpgradedProperty = statsInCategoryProperty.GetArrayElementAtIndex(j);

            FillModifiersList(statUpgradedProperty, additiveValue, multiplicativeValue, ref modifiersList, ref testValue);
          }
        }
      }

      EditorGUILayout.TextArea(modifiersList, helpBoxStyle);
    }

    private void FillModifiersList( 
      SerializedProperty statUpgradedProperty, SerializedProperty additiveValue, SerializedProperty multiplicativeValue, 
      ref string modifiersList, ref float testValue )
    {
      if (statUpgradedProperty.objectReferenceValue != null)
      {
        string assetPath = AssetDatabase.GetAssetPath(statUpgradedProperty.objectReferenceValue);
        string folderPath = assetPath.Replace($"/{statUpgradedProperty.objectReferenceValue.name}.asset", "");

        string folderName = string.Empty;

        // Extract the folder name
        for (int i = folderPath.Length - 1; i >= 0 && folderPath[i] != '/'; --i)
          folderName = folderPath[i] + folderName;

        modifiersList += $"\n• {folderName} - <i>{statUpgradedProperty.objectReferenceValue.name}</i>: ";

        if (additiveValue.floatValue != 0f) modifiersList += $"+{additiveValue.floatValue.ToString("F2")} ";
        if (multiplicativeValue.floatValue != 0f) modifiersList += $"+{(multiplicativeValue.floatValue * 100f).ToString("F0")}%";
        modifiersList += $"\n   {testValue} => <b>{(testValue + additiveValue.floatValue) * (1f + multiplicativeValue.floatValue)}</b>";
      }
    }
  }
}
