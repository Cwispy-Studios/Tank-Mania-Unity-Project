using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Upgrades
{
  [CustomEditor(typeof(Upgrade), true)]
  public class UpgradeEditor : Editor
  {
    private SerializedProperty upgradeImage;
    private SerializedProperty upgradeName;
    private SerializedProperty upgradeDescription;
    private SerializedProperty statModifiers;

    //private static string[] PropertiesInBaseClass = new string[] { 
    //  nameof(Upgrade.UpgradeImage), nameof(Upgrade.UpgradeName), nameof(Upgrade.UpgradeDescription), "m_Script" };

    private void OnEnable()
    {
      upgradeImage = serializedObject.FindProperty(nameof(Upgrade.UpgradeImage));
      upgradeName = serializedObject.FindProperty(nameof(Upgrade.UpgradeName));
      upgradeDescription = serializedObject.FindProperty(nameof(Upgrade.UpgradeDescription));
      statModifiers = serializedObject.FindProperty(nameof(Upgrade.StatModifiers));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      // Draws the disabled Script property at the top of every ScriptableObject
      EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
      EditorGUI.EndDisabledGroup();

      EditorGUILayout.PropertyField(upgradeName);
      upgradeImage.objectReferenceValue = EditorGUILayout.ObjectField("Image", upgradeImage.objectReferenceValue, typeof(Sprite), false);
      EditorGUILayout.PropertyField(upgradeDescription);

      if (GUILayout.Button("Upgrade Name", GUILayout.ExpandWidth(false))) 
        upgradeDescription.stringValue += $"{upgradeName.stringValue} ";

      EditorGUILayout.Space();

      EditorGUILayout.PropertyField(statModifiers, new GUIContent("Upgrade Components"));

      serializedObject.ApplyModifiedProperties();

      DrawModifierProperties();
    }

    private void DrawModifierProperties()
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
        StatModifier statModifier = statModifiers.GetArrayElementAtIndex(i).objectReferenceValue as StatModifier;

        if (statModifier)
        {
          modifiersList += $"\n• <i>{statModifier.name.Replace(" Modifier", "")}</i>: ";

          if (statModifier.AddititiveValue != 0f) modifiersList += $"+{statModifier.AddititiveValue.ToString("F2")} ";
          if (statModifier.MultiplicativeValue != 0f) modifiersList += $"+{(statModifier.MultiplicativeValue * 100f).ToString("F0")}%";
        }
      }

      EditorGUILayout.TextArea(modifiersList, helpBoxStyle);
    }
  }
}
