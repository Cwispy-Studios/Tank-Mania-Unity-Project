using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(Stat), true)]
  public class StatPropertyDrawer : PropertyDrawer
  {
    public const string UseIntPropertyName = "useInt";
    public const string BaseValuePropertyName = "baseValue";

    private GUIContent lockedButtonContent;
    private GUIContent unlockedButtonContent;

    private GUIStyle lockButtonStyle;

    private bool lockObjectField = true;
    private bool objectAssigned;

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
      float height = base.GetPropertyHeight(property, label);

      if (property.serializedObject.targetObject.GetType() == typeof(StatsCategory)) return height;

      objectAssigned = property.objectReferenceValue != null;

      if (objectAssigned)
      {
        height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      }

      return height;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
      if (property.serializedObject.targetObject.GetType().IsSubclassOf(typeof(StatsGroup)))
        DrawForStatsGroup(position, property, label);

      else DrawForStatsCategory(position, property, label);
    }

    private void DrawForStatsCategory( Rect position, SerializedProperty property, GUIContent label )
    {
      label = EditorGUI.BeginProperty(position, label, property);

      if (property.objectReferenceValue == null)
        label.text = "Unassigned";

      else
      {
        string assetPath = AssetDatabase.GetAssetPath(property.objectReferenceValue);
        string folderPath = assetPath.Replace($"/{property.objectReferenceValue.name}.asset", "");

        string folderName = string.Empty;

        // Extract the folder name
        for (int i = folderPath.Length - 1; i >= 0 && folderPath[i] != '/'; --i)
          folderName = folderPath[i] + folderName;

        label.text = folderName;
      }

      EditorGUI.PropertyField(position, property, label);
    }

    private void DrawForStatsGroup( Rect position, SerializedProperty property, GUIContent label )
    {
      // Initialise lock button graphics and style
      if (lockedButtonContent == null || unlockedButtonContent == null || lockButtonStyle == null)
      {
        lockedButtonContent = EditorGUIUtility.IconContent("LockIcon-On");
        unlockedButtonContent = EditorGUIUtility.IconContent("LockIcon");

        lockButtonStyle = GUI.skin.GetStyle("PaneOptions");
        // Sets the graphic to take up the entire button space
        lockButtonStyle.imagePosition = ImagePosition.ImageOnly;
      }

      label = EditorGUI.BeginProperty(position, label, property);

      position.height = EditorGUIUtility.singleLineHeight;

      // Rect for the lock button
      Rect lockButtonRect = position;
      lockButtonRect.x -= 12f;
      lockButtonRect.width = 12f;

      GUIContent lockButtonContent = lockObjectField ? lockedButtonContent : unlockedButtonContent;

      // Draw the lock button
      if (GUI.Button(lockButtonRect, lockButtonContent, lockButtonStyle)) lockObjectField = !lockObjectField;

      EditorGUI.LabelField(position, label);

      bool guiStatus = GUI.enabled;

      GUI.enabled = !lockObjectField;
      EditorGUI.PropertyField(position, property, new GUIContent(" "));
      GUI.enabled = guiStatus;

      // Do not need to show or initialise any values if stat is not assigned yet.
      if (!objectAssigned) return;

      SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
      serializedObject.Update();
      SetStatType(serializedObject);

      EditorGUI.BeginChangeCheck();

      Rect buttonRect = EditorGUI.IndentedRect(position);
      buttonRect.width = 50f;
      buttonRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      SerializedProperty useInt = serializedObject.FindProperty("useInt");

      DrawStatTypeButton(buttonRect, useInt);

      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

      DrawValueField(position, serializedObject, label);

      if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

      EditorGUI.EndProperty();
    }

    private void DrawStatTypeButton( Rect position, SerializedProperty useInt )
    {
      bool guiStatus = GUI.enabled;

      GUI.enabled = false;

      string text = useInt.boolValue ? "int" : "float";

      Color color = GUI.backgroundColor;
      GUI.backgroundColor = useInt.boolValue ? new Color(1f, 0.839f, 0.31f) : Color.cyan;

      GUI.Button(position, text);

      GUI.backgroundColor = color;
      GUI.enabled = guiStatus;
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
