using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof(StatModifier), true)]
  public class StatModifierEditor : Editor
  {
    private SerializedProperty additiveValue;
    private SerializedProperty multiplicativeValue;

    private SerializedProperty statsModified;

    private GUIStyle modifiersListHelpBoxStyle;
    private float baseValue = 10f;
    
    private void OnEnable()
    {
      additiveValue = serializedObject.FindProperty(nameof(additiveValue));
      multiplicativeValue = serializedObject.FindProperty(nameof(multiplicativeValue));

      statsModified = serializedObject.FindProperty(nameof(statsModified));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.LabelField($"{target.name}", EditorStyles.boldLabel);

      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.LabelField("Additive:", GUILayout.Width(55f));
      EditorGUILayout.PropertyField(additiveValue, new GUIContent(), GUILayout.Width(50f));

      if (GUILayout.Button("-10", GUILayout.ExpandWidth(false))) additiveValue.floatValue -= 10f;
      if (GUILayout.Button("-1", GUILayout.ExpandWidth(false))) additiveValue.floatValue -= 1f;
      if (GUILayout.Button("+1", GUILayout.ExpandWidth(false))) additiveValue.floatValue += 1f;
      if (GUILayout.Button("+10", GUILayout.ExpandWidth(false))) additiveValue.floatValue += 10f;

      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();

      multiplicativeValue.floatValue =
        EditorGUILayout.IntSlider(new GUIContent("Multiplicative:"),
          Mathf.RoundToInt(multiplicativeValue.floatValue * 100f) + 100, 0, 1000) * 0.01f - 1f;

      EditorGUILayout.LabelField("%", GUILayout.Width(15f));

      EditorGUILayout.EndHorizontal();

      EditorGUILayout.Space();

      GUI.backgroundColor = Color.red;

      if (GUILayout.Button("RESET", GUILayout.ExpandWidth(false))) 
      {
        additiveValue.floatValue = 0f;
        multiplicativeValue.floatValue = 0f;
        baseValue = 10f;
      }

      serializedObject.ApplyModifiedProperties();

      GUI.backgroundColor = GUI.color;

      EditorGUILayout.Space();

      EditorGUILayout.LabelField("Calculation: (baseValue + additiveValue) * multiplicativeValue", 
        EditorStyles.wordWrappedLabel);

      EditorGUILayout.Space();

      baseValue = EditorGUILayout.FloatField("Example Base Value:", baseValue);

      EditorGUILayout.LabelField($"Calculated Final Value: " +
        $"{((baseValue + additiveValue.floatValue) * (1f + multiplicativeValue.floatValue)).ToString("F2")}",
        EditorStyles.boldLabel);

      DrawModifiesList();
    }

    private void DrawModifiesList()
    {
      EditorGUILayout.Space();

      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      EditorGUILayout.Space();

      modifiersListHelpBoxStyle = GUI.skin.GetStyle("HelpBox");
      modifiersListHelpBoxStyle.richText = true;

      int modifiesNumber = statsModified.arraySize;
      string listOfStats = $"<b>Modifies {modifiesNumber} Stats:</b>";

      if (statsModified.arraySize == 0) listOfStats += "\nNONE.";
      else
      {
        for (int i = 0; i < statsModified.arraySize; ++i)
          listOfStats += $"\n{i + 1}. {statsModified.GetArrayElementAtIndex(i).stringValue}";
      }

      EditorGUILayout.TextArea(listOfStats, modifiersListHelpBoxStyle);
    }
  }
}
