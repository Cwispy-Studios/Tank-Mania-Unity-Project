using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  using Upgrades;

  [CustomPropertyDrawer(typeof(Stat), true)]
  public class StatPropertyDrawer : PropertyDrawer
  {
    private const float StatTypeButtonWidth = 7f;

    public const string UseIntPropertyName = "useInt";
    public const string BaseValuePropertyName = "baseValue";

    private bool objectAssigned;

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      float height = base.GetPropertyHeight(property, label);

      objectAssigned = property.objectReferenceValue != null;

      if (objectAssigned)
      {
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      }

      return height;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      position.height = EditorGUIUtility.singleLineHeight;

      // Make space for the stat type button, but only if object has already been assigned
      Rect prefixLabelRect = position;
      if (objectAssigned)
      {
        prefixLabelRect.x += StatTypeButtonWidth;
        prefixLabelRect.width = EditorGUIUtility.labelWidth - StatTypeButtonWidth;
      }
      EditorGUI.LabelField(prefixLabelRect, label);

      GUI.enabled = false;
      EditorGUI.PropertyField(position, property);
      GUI.enabled = true;

      // Do not need to show or initialise any values if stat is not assigned yet.
      if (!objectAssigned) return;

      SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
      serializedObject.Update();
      SetStatType(serializedObject);

      EditorGUI.BeginChangeCheck();

      Rect buttonRect = EditorGUI.IndentedRect(position);
      buttonRect.width = 7f;

      SerializedProperty useInt = serializedObject.FindProperty("useInt");

      DrawStatTypeButton(buttonRect, useInt);

      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      DrawValueField(position, serializedObject, label);

      if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }

    private void DrawStatTypeButton( Rect position, SerializedProperty useInt )
    {
      GUI.enabled = false;

      string text = useInt.boolValue ? "i" : "f";

      Color color = GUI.backgroundColor;
      GUI.backgroundColor = useInt.boolValue ? new Color(1f, 0.839f, 0.31f) : Color.cyan;

      GUI.Button(position, text);

      GUI.backgroundColor = color;
      GUI.enabled = true;
    }

    public virtual void SetStatType( SerializedObject serializedObject )
    {
      // Default use float
      serializedObject.FindProperty(UseIntPropertyName).boolValue = false;
    }

    public virtual void DrawValueField( Rect position, SerializedObject serializedObject, GUIContent label )
    {
      SerializedProperty baseValue = serializedObject.FindProperty(BaseValuePropertyName);

      bool useInt = serializedObject.FindProperty(UseIntPropertyName).boolValue;

      if (useInt)
        baseValue.floatValue = EditorGUI.IntField(position, label, (int)baseValue.floatValue);

      else
        baseValue.floatValue = EditorGUI.FloatField(position, label, baseValue.floatValue);
    }
  }
}
