using UnityEngine;
using UnityEditor;

namespace CwispyStudios.TankMania.Terrain
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            MapGenerator mapGen = (MapGenerator)target;

            if (DrawDefaultInspector()) {
                if (mapGen.autoUpdate) {
                    mapGen.DrawMapInEditor();
                }
            }

            if (GUILayout.Button("Generate")) {
                mapGen.DrawMapInEditor();
            }
        }
    }
}
