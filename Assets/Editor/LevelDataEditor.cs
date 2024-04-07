using Dialog;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData), true)]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelData picker = target as LevelData;
        SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.linkedScenePath);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        SceneAsset newScene = EditorGUILayout.ObjectField("Linked Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;
        picker.levelUIData = EditorGUILayout.ObjectField("Level UI Data", picker.levelUIData, typeof(LevelUIData), false) as LevelUIData;
        picker.dialogLevelData = EditorGUILayout.ObjectField("Dialog Level Data", picker.dialogLevelData, typeof(DialogLevelData), false) as DialogLevelData;
        picker.levelDuration = EditorGUILayout.FloatField(new GUIContent("Level Duration", "In Seconds"), picker.levelDuration);
        picker.requiredScore = EditorGUILayout.IntField(new GUIContent("Required Score", "In Seconds"), picker.requiredScore);

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("linkedScenePath");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
