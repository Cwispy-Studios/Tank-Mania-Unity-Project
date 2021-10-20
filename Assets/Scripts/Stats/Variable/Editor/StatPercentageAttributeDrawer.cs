using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(StatPercentageAttribute))]
  public class StatPercentageAttributeDrawer : StatPropertyDrawer
  {
    private const int MinRange = 0;
    private const int MaxRange = 100;

    private const float SymbolWidth = 15f;

    private GUIStyle suffixStyle;

    public override void SetStatType( SerializedObject serializedObject )
    {
      // Percentages can't be int
      serializedObject.FindProperty(UseIntPropertyName).boolValue = false;
    }

    public override void DrawValueField( Rect position, SerializedObject serializedObject, GUIContent label )
    {
      if (suffixStyle == null)
      {
        suffixStyle = GUI.skin.label;
        suffixStyle.normal.textColor = Color.gray;
        suffixStyle.hover = suffixStyle.normal;
      }

      SerializedProperty baseValue = serializedObject.FindProperty(BaseValuePropertyName);

      // Create the percentage slider button
      baseValue.floatValue = EditorGUI.IntSlider(position, label, (int)(baseValue.floatValue * 100f), MinRange, MaxRange) * 0.01f;

      // Move the rect into the slider's field to print the % sign as a suffix
      position.x += position.width - SymbolWidth;
      position.width = SymbolWidth;

      // Need to use button with label skin for some reasons since LabelField does not work????
      GUI.Button(position, "%", suffixStyle);
    }
  }
}
