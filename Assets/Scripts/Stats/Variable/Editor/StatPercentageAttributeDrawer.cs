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

    public override void DrawValueField( Rect position, SerializedProperty baseValue, SerializedProperty useInt, GUIContent label )
    {
      if (suffixStyle == null)
      {
        suffixStyle = GUI.skin.label;
        suffixStyle.normal.textColor = Color.gray;
        suffixStyle.hover = suffixStyle.normal;
      }

      // Percentages can't be int
      if (useInt.boolValue) useInt.boolValue = false;

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
