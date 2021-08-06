using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [CustomEditor(typeof(Damage))]
  public class DamageEditor : Editor
  {
    SerializedProperty splashDamage;

    SerializedProperty hasSplashDamage;
    SerializedProperty splashRadius;
    SerializedProperty splashDamagePercentage;
    SerializedProperty hasSplashDamageRolloff;
    SerializedProperty minRadiusPercentageRolloff;
    SerializedProperty maxRadiusPercentageRolloff;
    SerializedProperty minRadiusDamagePercentageRolloff;
    SerializedProperty maxRadiusDamagePercentageRolloff;

    private void OnEnable()
    {
      splashDamage = serializedObject.FindProperty(nameof(SplashDamage));

      hasSplashDamage = splashDamage.FindPropertyRelative(nameof(SplashDamage.HasSplashDamage));
      splashRadius = splashDamage.FindPropertyRelative(nameof(SplashDamage.SplashRadius));
      splashDamagePercentage = splashDamage.FindPropertyRelative(nameof(SplashDamage.SplashDamagePercentage));
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

      splashRadius.floatValue = EditorGUILayout.Slider("Radius", splashRadius.floatValue, 0f, 50f);
      splashDamagePercentage.floatValue = EditorGUILayout.Slider("Damage %", splashDamagePercentage.floatValue, 0f, 1f);

      EditorGUILayout.Space();

      hasSplashDamageRolloff.boolValue = EditorGUILayout.BeginToggleGroup(
          new GUIContent("Damage Rolloff", "Allows damage to attenuate based on how close the projectile landed to the object."),
          hasSplashDamageRolloff.boolValue
        );

      EditorGUILayout.EndToggleGroup();

      EditorGUI.indentLevel = 2;

      EditorStyles.label.fontStyle = FontStyle.Bold;
      EditorGUILayout.LabelField("Radius Cutoff");
      EditorStyles.label.fontStyle = FontStyle.Normal;

      minRadiusPercentageRolloff.floatValue = EditorGUILayout.Slider(
          new GUIContent("Min Radius %", "Percentage distance from the original radius the rolloff starts."),
          minRadiusPercentageRolloff.floatValue,
          0f, maxRadiusPercentageRolloff.floatValue
        );

      maxRadiusPercentageRolloff.floatValue = EditorGUILayout.Slider(
          new GUIContent("Max Radius %", "Percentage distance from the original radius the rolloff ends."),
          maxRadiusPercentageRolloff.floatValue,
          minRadiusPercentageRolloff.floatValue, 1f
        );

      EditorStyles.label.fontStyle = FontStyle.Bold;
      EditorGUILayout.LabelField("Damage Percentages of Radius");
      EditorStyles.label.fontStyle = FontStyle.Normal;

      minRadiusDamagePercentageRolloff.floatValue = EditorGUILayout.Slider(
          new GUIContent("Min Damage %", "Percentage damage dealt where the rolloff starts."),
          minRadiusDamagePercentageRolloff.floatValue,
          0f, 1f
        );

      maxRadiusDamagePercentageRolloff.floatValue = EditorGUILayout.Slider(
          new GUIContent("Max Damage %", "Percentage damage dealt where the rolloff ends."),
          maxRadiusDamagePercentageRolloff.floatValue,
          0f, 1f
        );

      EditorGUI.indentLevel = 0;

      EditorGUILayout.EndToggleGroup();

      serializedObject.ApplyModifiedProperties();
    }
  }
}
