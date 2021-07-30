//using UnityEditor;
//using UnityEngine;

//namespace CwispyStudios.TankMania.Editor
//{
//  using Combat;

//  [CustomPropertyDrawer(typeof(DamageInformation))]
//  public class DamageInformationDrawer : PropertyDrawer
//  {
//    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
//    {
//      EditorGUI.BeginProperty(position, label, property);

//      Rect rect = position;
//      rect.height = EditorGUIUtility.singleLineHeight;

//      position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//      SerializedProperty directDamageProperty = property.FindPropertyRelative(nameof(DamageInformation.DirectDamage));
//      EditorGUI.Slider(rect, directDamageProperty, 0f, 10000f);

//      rect.y += EditorGUIUtility.singleLineHeight;

//      SerializedProperty splashDamageProperty = property.FindPropertyRelative(nameof(DamageInformation.SplashDamage));
//      EditorGUI.PropertyField(rect, splashDamageProperty);

//      EditorGUI.EndProperty();
//    }

//    //public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
//    //{
//    //  return 200f;
//    //}
//  }
//}
