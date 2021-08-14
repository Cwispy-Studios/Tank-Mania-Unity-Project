using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomEditor(typeof(Damage))]
  public class DamageEditor : Editor
  {
    private SerializedProperty splashDamage;

    private SerializedProperty hasSplashDamage;
    private SerializedProperty splashRadius;
    private SerializedProperty splashDamagePercentage;
    private SerializedProperty hasSplashDamageRolloff;
    private SerializedProperty minRadiusPercentageRolloff;
    private SerializedProperty maxRadiusPercentageRolloff;
    private SerializedProperty minRadiusDamagePercentageRolloff;
    private SerializedProperty maxRadiusDamagePercentageRolloff;

    private void OnEnable()
    {
      splashDamage = serializedObject.FindProperty(nameof(SplashDamage));

      hasSplashDamage = splashDamage.FindPropertyRelative(nameof(SplashDamage.HasSplashDamage));
      splashRadius = splashDamage.FindPropertyRelative(nameof(SplashDamage.Radius));
      splashDamagePercentage = splashDamage.FindPropertyRelative(nameof(SplashDamage.DamagePercentage));
      hasSplashDamageRolloff = splashDamage.FindPropertyRelative(nameof(SplashDamage.HasSplashDamageRolloff));
      minRadiusPercentageRolloff = splashDamage.FindPropertyRelative(nameof(SplashDamage.MinRadiusPercentageRolloff));
      maxRadiusPercentageRolloff = splashDamage.FindPropertyRelative(nameof(SplashDamage.MaxRadiusPercentageRolloff));
      minRadiusDamagePercentageRolloff = splashDamage.FindPropertyRelative(nameof(SplashDamage.MinRadiusDamagePercentageRolloff));
      maxRadiusDamagePercentageRolloff = splashDamage.FindPropertyRelative(nameof(SplashDamage.MaxRadiusDamagePercentageRolloff));
    }

    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      serializedObject.Update();

      EditorGUILayout.Space();

      EditorStyles.label.fontStyle = FontStyle.Bold;
      hasSplashDamage.boolValue = EditorGUILayout.BeginToggleGroup("Splash Damage", hasSplashDamage.boolValue);
      EditorStyles.label.fontStyle = FontStyle.Normal;

      EditorGUI.indentLevel = 1;

      EditorGUILayout.PropertyField(splashRadius, new GUIContent("Radius"));
      EditorGUILayout.PropertyField(splashDamagePercentage, new GUIContent("Damage %"));

      EditorGUILayout.Space();

      hasSplashDamageRolloff.boolValue = EditorGUILayout.BeginToggleGroup(
        new GUIContent("Damage Rolloff", "Allows damage to attenuate based on how close the projectile landed to the object."),
        hasSplashDamageRolloff.boolValue);

      EditorGUI.indentLevel = 2;

      // Min max radius % properties
      EditorGUILayout.PropertyField(minRadiusPercentageRolloff, new GUIContent("Min Radius %"));
      EditorGUILayout.PropertyField(maxRadiusPercentageRolloff, new GUIContent("Max Radius %"));

      // Check min max values if they are correct
      float minValue = minRadiusPercentageRolloff.FindPropertyRelative("baseValue").floatValue;
      float maxValue = maxRadiusPercentageRolloff.FindPropertyRelative("baseValue").floatValue;

      if (minValue > maxValue)
        EditorGUILayout.HelpBox("WARNING! The minimum radius is larger than the maximum radius!", MessageType.Warning, true);

      // Min max damage at radius % properties
      EditorGUILayout.PropertyField(minRadiusDamagePercentageRolloff, new GUIContent("Min Damage %"));
      EditorGUILayout.PropertyField(maxRadiusDamagePercentageRolloff, new GUIContent("Max Damage %"));

      // Check min max values and show note to player about potential unique behaviour
      minValue = minRadiusDamagePercentageRolloff.FindPropertyRelative("baseValue").floatValue;
      maxValue = maxRadiusDamagePercentageRolloff.FindPropertyRelative("baseValue").floatValue;

      if (minValue < maxValue)
        EditorGUILayout.HelpBox("Damage is now higher the further the target is from the blast.", MessageType.Info, true);

      EditorGUI.indentLevel = 0;

      EditorGUILayout.EndToggleGroup();

      EditorGUILayout.EndToggleGroup();

      serializedObject.ApplyModifiedProperties();
    }
  }
}
