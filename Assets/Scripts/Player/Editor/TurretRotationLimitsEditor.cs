using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CustomEditor(typeof(TurretRotationLimits))]
  public class TurretRotationLimitsEditor : Editor
  {
    private SerializedProperty HasXLimits;
    private SerializedProperty MinXRot;
    private SerializedProperty MaxXRot;
    private SerializedProperty HasYLimits;
    private SerializedProperty MinYRot;
    private SerializedProperty MaxYRot;

    private void OnEnable()
    {
      HasXLimits = serializedObject.FindProperty(nameof(TurretRotationLimits.HasXLimits));
      MinXRot = serializedObject.FindProperty(nameof(TurretRotationLimits.MinXRot));
      MaxXRot = serializedObject.FindProperty(nameof(TurretRotationLimits.MaxXRot));
      HasYLimits = serializedObject.FindProperty(nameof(TurretRotationLimits.HasYLimits));
      MinYRot = serializedObject.FindProperty(nameof(TurretRotationLimits.MinYRot));
      MaxYRot = serializedObject.FindProperty(nameof(TurretRotationLimits.MaxYRot));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.HelpBox("Max range is 360 degrees. Range must pass through 0.", MessageType.Info);

      EditorGUILayout.PropertyField(HasXLimits);

      if (HasXLimits.boolValue)
      {
        EditorGUI.indentLevel += 1;

        MinXRot.floatValue = EditorGUILayout.IntSlider("Min rotation", (int)MinXRot.floatValue,
          Mathf.Max(-360, (int)MaxXRot.floatValue - 360), Mathf.Min(0, (int)MaxXRot.floatValue));
        MaxXRot.floatValue = EditorGUILayout.IntSlider("Max rotation", (int)MaxXRot.floatValue, 
          Mathf.Max(0, (int)MinXRot.floatValue), Mathf.Min((int)MinXRot.floatValue + 360, 360));

        EditorGUI.indentLevel -= 1;
      }

      EditorGUILayout.PropertyField(HasYLimits);

      if (HasYLimits.boolValue)
      {
        EditorGUI.indentLevel += 1;

        MinYRot.floatValue = EditorGUILayout.IntSlider("Min rotation", (int)MinYRot.floatValue,
          Mathf.Max(-360, (int)MaxYRot.floatValue - 360), Mathf.Min(0, (int)MaxYRot.floatValue));
        MaxYRot.floatValue = EditorGUILayout.IntSlider("Max rotation", (int)MaxYRot.floatValue,
          Mathf.Max(0, (int)MinYRot.floatValue), Mathf.Min((int)MinYRot.floatValue + 360, 360));

        EditorGUI.indentLevel -= 1;
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}
