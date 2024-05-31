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
        picker.mapPreviewIcon = EditorGUILayout.ObjectField("Icon map", picker.mapPreviewIcon, typeof(Sprite), false) as Sprite;
        picker.levelID = EditorGUILayout.IntField(new GUIContent("Level ID (first is 0)", ""), picker.levelID);
        SceneAsset newScene = EditorGUILayout.ObjectField("Linked Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;
        picker.levelMusicData = EditorGUILayout.ObjectField("Level Music Data", picker.levelMusicData, typeof(LevelMusicData), false) as LevelMusicData;
        picker.levelUIData = EditorGUILayout.ObjectField("Level UI Data", picker.levelUIData, typeof(LevelUIData), false) as LevelUIData;
        picker.dialogLevelData = EditorGUILayout.ObjectField("Dialog Level Data", picker.dialogLevelData, typeof(DialogLevelData), false) as DialogLevelData;
        picker.requiredScore = EditorGUILayout.IntField(new GUIContent("Required Score", "the maximum stars is this x2"), picker.requiredScore);

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("linkedScenePath");
            scenePathProperty.stringValue = newPath;

            EditorUtility.SetDirty(picker);
            // AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
