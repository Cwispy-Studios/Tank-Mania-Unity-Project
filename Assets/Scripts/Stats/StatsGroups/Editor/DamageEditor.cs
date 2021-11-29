using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomEditor(typeof(Damage))]
  public class DamageEditor : StatsGroupEditor
  {
    private SerializedProperty directDamage;
    private SerializedProperty dudCollisionDamagePercentage;
    private SerializedProperty hasSplashDamage;
    private SerializedProperty splashRadius;
    private SerializedProperty splashDamagePercentage;
    private SerializedProperty hasSplashDamageRolloff;
    private SerializedProperty minRadiusPercentageRolloff;
    private SerializedProperty maxRadiusPercentageRolloff;
    private SerializedProperty minRadiusDamagePercentageRolloff;
    private SerializedProperty maxRadiusDamagePercentageRolloff;

    public override void OnEnable()
    {
      base.OnEnable();

      directDamage = serializedObject.FindProperty(nameof(Damage.DirectDamage));
      dudCollisionDamagePercentage = serializedObject.FindProperty(nameof(Damage.DudCollisionDamagePercentage));

      hasSplashDamage = serializedObject.FindProperty(nameof(Damage.HasSplashDamage));
      splashRadius = serializedObject.FindProperty(nameof(Damage.SplashRadius));
      splashDamagePercentage = serializedObject.FindProperty(nameof(Damage.SplashDamagePercentage));
      hasSplashDamageRolloff = serializedObject.FindProperty(nameof(Damage.HasSplashDamageRolloff));
      minRadiusPercentageRolloff = serializedObject.FindProperty(nameof(Damage.SplashMinRadiusPercentageRolloff));
      maxRadiusPercentageRolloff = serializedObject.FindProperty(nameof(Damage.SplashMaxRadiusPercentageRolloff));
      minRadiusDamagePercentageRolloff = serializedObject.FindProperty(nameof(Damage.SplashMinRadiusDamagePercentageRolloff));
      maxRadiusDamagePercentageRolloff = serializedObject.FindProperty(nameof(Damage.SplashMaxRadiusDamagePercentageRolloff));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(directDamage);
      EditorGUILayout.PropertyField(dudCollisionDamagePercentage, new GUIContent("Dud Damage %"));

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
      float minValue = minRadiusPercentageRolloff.objectReferenceValue == null ? 0f :
        (minRadiusPercentageRolloff.objectReferenceValue as Stat).Value;
      float maxValue = maxRadiusPercentageRolloff.objectReferenceValue == null ? 1f :
        (maxRadiusPercentageRolloff.objectReferenceValue as Stat).Value;

      if (minValue > maxValue)
        EditorGUILayout.HelpBox("WARNING! The minimum radius is larger than the maximum radius!", MessageType.Warning, true);

      // Min max damage at radius % properties
      EditorGUILayout.PropertyField(minRadiusDamagePercentageRolloff, new GUIContent("Min Damage %"));
      EditorGUILayout.PropertyField(maxRadiusDamagePercentageRolloff, new GUIContent("Max Damage %"));

      // Check min max values and show note to player about potential unique behaviour
      minValue = minRadiusDamagePercentageRolloff.objectReferenceValue == null ? 1f :
        (minRadiusDamagePercentageRolloff.objectReferenceValue as Stat).Value;
      maxValue = maxRadiusDamagePercentageRolloff.objectReferenceValue == null ? 0f :
        (maxRadiusDamagePercentageRolloff.objectReferenceValue as Stat).Value;

      if (minValue < maxValue)
        EditorGUILayout.HelpBox("Damage is now higher the further the target is from the blast.", MessageType.Info, true);

      EditorGUI.indentLevel = 0;

      EditorGUILayout.EndToggleGroup();

      EditorGUILayout.EndToggleGroup();

      serializedObject.ApplyModifiedProperties();

      DrawAssetManagementOptions();
    }
  }
}
