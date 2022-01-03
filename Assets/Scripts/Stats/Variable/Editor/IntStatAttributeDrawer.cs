using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomPropertyDrawer(typeof(IntStatAttribute))]
  public class IntStatAttributeDrawer : StatPropertyDrawer
  {
    public override void SetStatType( SerializedObject serializedObject )
    {
      serializedObject.FindProperty(UseIntPropertyName).boolValue = true;
    }
  }
}
