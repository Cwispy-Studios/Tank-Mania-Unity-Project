using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CustomEditor(typeof(Stat))]
  public class StatEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      GUI.enabled = false;
      base.OnInspectorGUI();
      GUI.enabled = true;
    }
  }
}
