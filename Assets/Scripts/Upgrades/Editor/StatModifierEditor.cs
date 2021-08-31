using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [CanEditMultipleObjects]
  [CustomEditor(typeof(StatModifier), true)]
  public class StatModifierEditor : Editor
  {
    private SerializedProperty additiveValue;
    private SerializedProperty multiplicativeValue;

    private SerializedProperty statsGroupsModified;
    private SerializedProperty statNamesModified;

    private float baseValue = 10f;
    
    private void OnEnable()
    {
      additiveValue = serializedObject.FindProperty(nameof(additiveValue));
      multiplicativeValue = serializedObject.FindProperty(nameof(multiplicativeValue));

      statsGroupsModified = serializedObject.FindProperty(nameof(statsGroupsModified));
      statNamesModified = serializedObject.FindProperty(nameof(statNamesModified));

      // Required when a stat modifier is removed from a stat
      (serializedObject.targetObject as StatModifier).RefreshStatsList();
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

      // Draw a seperator line
      Rect lineRect = EditorGUILayout.GetControlRect(false, 1f);
      EditorGUI.DrawRect(lineRect, Color.gray);

      EditorGUILayout.Space();

      // The modifiers list will be drawn in a help box style
      GUIStyle modifiersListHelpBoxStyle = GUI.skin.GetStyle("HelpBox");

      // Retrieve font information only
      GUIStyle modifiersListTextStyle = GUI.skin.label;
      modifiersListTextStyle.font = modifiersListTextStyle.font;
      modifiersListTextStyle.fontSize = modifiersListHelpBoxStyle.fontSize;
      modifiersListTextStyle.fontStyle = modifiersListHelpBoxStyle.fontStyle;
      modifiersListTextStyle.richText = true;

      // Categorise all the stat names inside their stats group
      Dictionary<StatsGroup, List<string>> statsCategorised = new Dictionary<StatsGroup, List<string>>();

      for (int i = 0; i < statsGroupsModified.arraySize; ++i)
      {
        StatsGroup statsGroup = statsGroupsModified.GetArrayElementAtIndex(i).objectReferenceValue as StatsGroup;

        if (!statsCategorised.ContainsKey(statsGroup))
        {
          statsCategorised.Add(statsGroup, new List<string>());
        }

        statsCategorised[statsGroup].Add(statNamesModified.GetArrayElementAtIndex(i).stringValue);
      }

      // Create help box for all elements below this
      EditorGUILayout.BeginVertical(modifiersListHelpBoxStyle);

      int numberOfStatsGroups = statsCategorised.Keys.Count;
      int numberOfStats = statNamesModified.arraySize;

      string labelText = $"<b>Modifies <i>{numberOfStats} Stats</i> in <i>{numberOfStatsGroups} Stats Group(s)</i>:</b>";

      EditorGUILayout.TextArea(labelText, modifiersListTextStyle);

      if (statNamesModified.arraySize == 0) EditorGUILayout.LabelField("NONE.");
      else
      {
        int statsGroupsIndex = 0;
        int statsIndex = 0;

        // Loop through every unique stat group....
        foreach (StatsGroup statsGroup in statsCategorised.Keys)
        {
          // Track the index of the stat group
          ++statsGroupsIndex;

          // Create help box for each stat group's stats
          EditorGUILayout.BeginVertical(modifiersListHelpBoxStyle);

          // Draw the numerical label and an inactive object field of the stat group on the same line
          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.LabelField($"{statsGroupsIndex}.", GUILayout.Width(20f));
          GUI.enabled = false;
          EditorGUILayout.ObjectField(string.Empty, statsGroup, typeof(StatsGroup), false);
          GUI.enabled = true;
          EditorGUILayout.EndHorizontal();

          List<string> statNamesInGroup = statsCategorised[statsGroup];

          foreach (string statName in statNamesInGroup)
          {
            ++statsIndex;

            EditorGUILayout.TextArea($"Stat {statsIndex}. {statName}", modifiersListTextStyle);
          }

          EditorGUILayout.EndVertical();
        }
      }

      EditorGUILayout.EndVertical();
    }
  }
}
