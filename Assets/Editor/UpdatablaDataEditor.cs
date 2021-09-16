using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Terrain
{
    [CustomEditor(typeof(UpdatableData), true)]
    public class UpdatablaDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UpdatableData data = (UpdatableData)target;
            if (GUILayout.Button("Update")) 
            {
                data.NotifyOfUpdatedValues();
                EditorUtility.SetDirty(target); 
            }
        }
    }
}