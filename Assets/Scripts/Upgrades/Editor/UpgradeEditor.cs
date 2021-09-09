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
    private SerializedProperty playerStatModifiers;
    private SerializedProperty enemyStatModifiers;

    private void OnEnable()
    {
      upgradeRarity = serializedObject.FindProperty(nameof(Upgrade.UpgradeRarity));
      upgradeImage = serializedObject.FindProperty(nameof(Upgrade.UpgradeImage));
      upgradeName = serializedObject.FindProperty(nameof(Upgrade.UpgradeName));
      upgradeDescription = serializedObject.FindProperty(nameof(Upgrade.UpgradeDescription));
      playerStatModifiers = serializedObject.FindProperty(nameof(Upgrade.PlayerStatModifiers));
      enemyStatModifiers = serializedObject.FindProperty(nameof(Upgrade.EnemyStatModifiers));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      // Draws the disabled Script property at the top of every ScriptableObject
      EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
      EditorGUI.EndDisabledGroup();

      EditorGUILayout.PropertyField(upgradeRarity);

      if (serializedObject.isEditingMultipleObjects)
      {
        serializedObject.ApplyModifiedProperties();
        return;
      }

      EditorGUILayout.PropertyField(upgradeName);
      upgradeImage.objectReferenceValue = EditorGUILayout.ObjectField("Image", upgradeImage.objectReferenceValue, typeof(Sprite), false);
      EditorGUILayout.PropertyField(upgradeDescription);

      if (GUILayout.Button("Upgrade Name", GUILayout.ExpandWidth(false))) 
        upgradeDescription.stringValue += $"{upgradeName.stringValue} ";

      EditorGUILayout.Space();

      if (playerStatModifiers.arraySize > 0 || enemyStatModifiers.arraySize > 0)
        EditorGUILayout.HelpBox(
          "NOTE! Remove all stat modifier instances before deleting the asset to remove all references to them from the stats subscriptions!",
          MessageType.Warning);

      EditorGUILayout.PropertyField(playerStatModifiers, new GUIContent("Player Upgrade Components"));

      DrawModifierProperties(playerStatModifiers);

      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(enemyStatModifiers, new GUIContent("Enemy Upgrade Components"));

      DrawModifierProperties(enemyStatModifiers);

      serializedObject.ApplyModifiedProperties();
    }

    private void DrawModifierProperties( SerializedProperty statModifiers )
    {
      EditorGUILayout.Space();

      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      EditorGUILayout.Space();

      GUIStyle helpBoxStyle = GUI.skin.GetStyle("HelpBox");
      helpBoxStyle.richText = true;

      string modifiersList = $"<b>Modifiers List:</b>";

      for (int i = 0; i < statModifiers.arraySize; ++i)
      {
        SerializedProperty instanceProperty = statModifiers.GetArrayElementAtIndex(i);
        SerializedProperty nameProperty = instanceProperty.FindPropertyRelative("instanceName");
        SerializedProperty modiferProperty = instanceProperty.FindPropertyRelative("statModifier");

        StatModifier statModifier = modiferProperty.objectReferenceValue as StatModifier;

        if (statModifier != null)
        {
          modifiersList += $"\n� <i>{nameProperty.stringValue.Replace(" Modifier", "")}</i>: ";

          if (statModifier.AddititiveValue != 0f) modifiersList += $"+{statModifier.AddititiveValue.ToString("F2")} ";
          if (statModifier.MultiplicativeValue != 0f) modifiersList += $"+{(statModifier.MultiplicativeValue * 100f).ToString("F0")}%";
        }
      }

      // Does not work :<
      //modifiersList.Replace("+-", "-");

      EditorGUILayout.TextArea(modifiersList, helpBoxStyle);
    }
  }
}
