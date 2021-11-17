using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(FloatStatAttribute))]
  public class FloatStatAttributeDrawer : StatPropertyDrawer
  {
    public override void SetStatType( SerializedObject serializedObject )
    {
      serializedObject.FindProperty(UseIntPropertyName).boolValue = false;
    }
  }
}
