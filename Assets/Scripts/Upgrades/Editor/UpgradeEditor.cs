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

    private static string[] PropertiesInBaseClass = new string[] { 
      nameof(Upgrade.UpgradeImage), nameof(Upgrade.UpgradeName), nameof(Upgrade.UpgradeDescription), "m_Script" };

    private void OnEnable()
    {
      upgradeImage = serializedObject.FindProperty(PropertiesInBaseClass[0]);
      upgradeName = serializedObject.FindProperty(PropertiesInBaseClass[1]);
      upgradeDescription = serializedObject.FindProperty(PropertiesInBaseClass[2]);
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      // Draws the disabled Script property at the top of every ScriptableObject
      EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
      EditorGUI.EndDisabledGroup();

      EditorGUILayout.PropertyField(upgradeName);
      EditorGUILayout.ObjectField("Image", upgradeImage.objectReferenceValue, typeof(Sprite), false);
      EditorGUILayout.PropertyField(upgradeDescription);

      //EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("Upgrade Name", GUILayout.ExpandWidth(false))) 
        upgradeDescription.stringValue += $"{upgradeName.stringValue} ";
      //EditorGUILayout.EndHorizontal();

      // Draw default inherited classes properties
      DrawPropertiesExcluding(serializedObject, PropertiesInBaseClass);

      serializedObject.ApplyModifiedProperties();
    }
  }
}
