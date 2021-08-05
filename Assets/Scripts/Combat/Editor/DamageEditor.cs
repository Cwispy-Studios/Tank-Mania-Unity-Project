using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [CustomEditor(typeof(Damage))]
  public class DamageEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      Damage damage = (Damage)target;
      SplashDamage splashDamage = damage.SplashDamage;

      EditorGUILayout.Space();

      EditorStyles.label.fontStyle = FontStyle.Bold;
      splashDamage.HasSplashDamage = 
        EditorGUILayout.Toggle("Splash Damage", splashDamage.HasSplashDamage);
      EditorStyles.label.fontStyle = FontStyle.Normal;

      if (splashDamage.HasSplashDamage)
      {
        EditorGUI.indentLevel = 1;

        splashDamage.SplashRadius = 
          EditorGUILayout.Slider("Radius", splashDamage.SplashRadius, 0f, 50f);
        splashDamage.SplashDamagePercentage = 
          EditorGUILayout.Slider("Damage %", splashDamage.SplashDamagePercentage, 0f, 1f);

        EditorGUILayout.Space();

        splashDamage.HasSplashDamageRolloff = 
          EditorGUILayout.Toggle(
            new GUIContent("Damage Rolloff", "Allows damage to attenuate based on how close the projectile landed to the object."), 
            splashDamage.HasSplashDamageRolloff
            );

        if (splashDamage.HasSplashDamageRolloff)
        {
          EditorGUI.indentLevel = 2;

          EditorStyles.label.fontStyle = FontStyle.Bold;
          EditorGUILayout.LabelField("Radius Cutoff");
          EditorStyles.label.fontStyle = FontStyle.Normal;

          splashDamage.MinRadiusPercentageRolloff =
            EditorGUILayout.Slider(
              new GUIContent("Min Radius %", "Percentage distance from the original radius the rolloff starts."),
              splashDamage.MinRadiusPercentageRolloff,
              0f, splashDamage.MaxRadiusPercentageRolloff
            );

          splashDamage.MaxRadiusPercentageRolloff =
            EditorGUILayout.Slider(
              new GUIContent("Max Radius %", "Percentage distance from the original radius the rolloff ends."),
              splashDamage.MaxRadiusPercentageRolloff,
              splashDamage.MinRadiusPercentageRolloff, 1f);

          EditorStyles.label.fontStyle = FontStyle.Bold;
          EditorGUILayout.LabelField("Damage Percentages of Radius");
          EditorStyles.label.fontStyle = FontStyle.Normal;

          splashDamage.MinRadiusDamagePercentageRolloff =
            EditorGUILayout.Slider(
              new GUIContent("Min Damage %", "Percentage damage dealt where the rolloff starts."),
              splashDamage.MinRadiusDamagePercentageRolloff,
              0f, 1f
              );

          splashDamage.MaxRadiusDamagePercentageRolloff =
            EditorGUILayout.Slider(
              new GUIContent("Max Damage %", "Percentage damage dealt where the rolloff ends."),
              splashDamage.MaxRadiusDamagePercentageRolloff,
              0f, 1f
              );

          EditorGUI.indentLevel = 0;
        }
      }
    }
  }
}
