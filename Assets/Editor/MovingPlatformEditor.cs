using System;
using System.Collections.Generic;
using Level.LevelDesign;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform))]
public class LookAtPointEditor : Editor
{
    MovingPlatform _platform;
    SerializedProperty _keyList;

    List<MovingPlatformKey> _tempKeyList = new List<MovingPlatformKey>();
    
    void OnEnable()
    {
        _platform = target as MovingPlatform;
        _keyList = serializedObject.FindProperty("platformKeys");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        if (GUILayout.Button("Add Key"))
        {
            //_keyList.Add(new MovingPlatformKey(_platform.transform.position, _platform.transform.rotation, 0, 1));
            //_keyList.InsertArrayElementAtIndex(_keyList.arraySize);
            //_keyList.GetArrayElementAtIndex(_keyList.arraySize - 1). = new MovingPlatformKey(_platform.transform.position, _platform.transform.rotation, 0, 1);
        }

        for (int i = 0; i < _tempKeyList.Count; i++)
        {
            
        }

        EditorGUI.EndChangeCheck();
        serializedObject.ApplyModifiedProperties();
    }
}