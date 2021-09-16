using UnityEditor;
using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CustomEditor(typeof(AmmoUI))]
  public class AmmoUIEditor : Editor
  {
    private SerializedProperty useStackedAmmoImages;
    private SerializedProperty ammoImageGroupPrefab;
    private SerializedProperty ammoImagePrefab;
    private SerializedProperty firingInformation;
    private SerializedProperty firingCountdown;
    private SerializedProperty reloadCountdown;
    private SerializedProperty currentAmmo;

    private void OnEnable()
    {
      useStackedAmmoImages = serializedObject.FindProperty(nameof(useStackedAmmoImages));
      ammoImageGroupPrefab = serializedObject.FindProperty(nameof(ammoImageGroupPrefab));
      ammoImagePrefab = serializedObject.FindProperty(nameof(ammoImagePrefab));
      firingInformation = serializedObject.FindProperty(nameof(firingInformation));
      reloadCountdown = serializedObject.FindProperty(nameof(reloadCountdown));
      currentAmmo = serializedObject.FindProperty(nameof(currentAmmo));
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(useStackedAmmoImages);
      EditorGUILayout.PropertyField(useStackedAmmoImages.boolValue ? ammoImageGroupPrefab : ammoImagePrefab);
      EditorGUILayout.PropertyField(firingInformation);
      EditorGUILayout.PropertyField(reloadCountdown);
      EditorGUILayout.PropertyField(currentAmmo);

      serializedObject.ApplyModifiedProperties();
    }
  }
}
