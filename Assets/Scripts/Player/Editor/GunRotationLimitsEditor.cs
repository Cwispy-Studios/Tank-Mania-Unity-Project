using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CustomEditor(typeof(TurretRotationLimits))]
  public class GunRotationLimitsEditor : Editor
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

      EditorGUILayout.HelpBox("Min value can be greater than max value, range of limits: >=min && <=max.", MessageType.Info);

      EditorGUILayout.PropertyField(HasXLimits);

      if (HasXLimits.boolValue)
      {
        EditorGUI.indentLevel += 1;

        MinXRot.floatValue = MathHelper.ConvertToSignedAngle(EditorGUILayout.IntSlider("Min rotation", (int)MinXRot.floatValue, -180, (int)MaxXRot.floatValue));
        MaxXRot.floatValue = MathHelper.ConvertToSignedAngle(EditorGUILayout.IntSlider("Max rotation", (int)MaxXRot.floatValue, (int)MinXRot.floatValue, 180));

        EditorGUI.indentLevel -= 1;
      }

      EditorGUILayout.PropertyField(HasYLimits);

      if (HasYLimits.boolValue)
      {
        EditorGUI.indentLevel += 1;
        MinYRot.floatValue = MathHelper.ConvertToSignedAngle(EditorGUILayout.IntSlider("Min rotation", (int)MinYRot.floatValue, -180, (int)MaxYRot.floatValue));
        MaxYRot.floatValue = MathHelper.ConvertToSignedAngle(EditorGUILayout.IntSlider("Max rotation", (int)MaxYRot.floatValue, (int)MinYRot.floatValue, 180));
        EditorGUI.indentLevel -= 1;
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}
