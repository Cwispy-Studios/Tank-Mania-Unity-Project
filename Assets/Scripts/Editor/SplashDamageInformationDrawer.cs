//using UnityEditor;
//using UnityEngine;

//namespace CwispyStudios.TankMania.Editor
//{
//  using Combat;

//  [CustomPropertyDrawer(typeof(SplashDamageInformation))]
//  public class SplashDamageInformationDrawer : PropertyDrawer
//  {
//    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
//    {
//      EditorGUI.BeginProperty(position, label, property);

//      SerializedProperty hasSplashDamageProperty = property.FindPropertyRelative(nameof(SplashDamageInformation.HasSplashDamage));
//      EditorGUI.PropertyField(position, hasSplashDamageProperty);

//      // Do not display splash damage information
//      if (!hasSplashDamageProperty.boolValue) return;


//    }
//  }
//}
