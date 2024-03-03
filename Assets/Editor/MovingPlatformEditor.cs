using Level.LevelDesign;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatform))]
public class LookAtPointEditor : Editor
{
    MovingPlatform _platform;

    int _selectedIndex = 0;
    float _previewT = 0;

    #region GUI BUTTON ENABLER

    bool _previousKeyEnabled;
    bool _nextKeyEnabled;
    bool _clearEnabled;
    bool _removeEnabled;
    bool _startPreview;

    #endregion
    
    void OnEnable()
    {
        _platform = target as MovingPlatform;
        if (_platform.platformKeys.Count > 0)
            _selectedIndex = 1;
        UpdateButtonEnabling();
    }

    void OnDisable()
    {
        if (_platform.platformKeys.Count > 0)
            SetPlatformPositionToKey(0);
    }

    public override void OnInspectorGUI()
    {
        if (!EditorGUIUtility.wideMode)
        {
            EditorGUIUtility.wideMode = true;
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
        }
        
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        #region Add/Insert/Remove Key Button

        GUILayout.BeginHorizontal("box");
        
        if (_selectedIndex == _platform.platformKeys.Count || _selectedIndex == 0)
        {
            if (GUILayout.Button("Add Key")) { AddKey(); }
        }
        else
        {
            if (GUILayout.Button("Insert Key")) { InsertKey(); }
        }

        GUI.enabled = _removeEnabled;
        if (GUILayout.Button("Remove Key")) { RemoveKey(); }
        GUI.enabled = true;
        
        GUILayout.EndHorizontal();

        #endregion

        #region Navigation Buttons

        GUILayout.BeginHorizontal("box");

        GUI.enabled = _previousKeyEnabled;
        if (GUILayout.Button("<")) { GoToPreviousKey(); }
        GUILayout.FlexibleSpace();
        GUI.enabled = true;
        GUILayout.Label($"Key {_selectedIndex} of {_platform.platformKeys.Count.ToString()}",EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUI.enabled = _nextKeyEnabled;
        if (GUILayout.Button(">")) { GoToNextKey(); }
        GUI.enabled = true;

        GUILayout.EndHorizontal();

        #endregion

        #region Key properties

        if (_selectedIndex > 0)
        {
            _platform.platformKeys[_selectedIndex-1].position = 
                EditorGUILayout.Vector3Field("Position", _platform.platformKeys[_selectedIndex-1].position);
            _platform.platformKeys[_selectedIndex-1].rotation = 
                EditorGUILayout.Vector3Field("Rotation", _platform.platformKeys[_selectedIndex-1].rotation);
            _platform.platformKeys[_selectedIndex-1].scale = 
                EditorGUILayout.Vector3Field("Scale", _platform.platformKeys[_selectedIndex-1].scale);
            _platform.platformKeys[_selectedIndex - 1].pauseBeforeMoving =
                EditorGUILayout.FloatField("Pause (seconds)",
                    _platform.platformKeys[_selectedIndex - 1].pauseBeforeMoving);
            _platform.platformKeys[_selectedIndex - 1].travelTime =
                EditorGUILayout.FloatField("Time To Travel", _platform.platformKeys[_selectedIndex - 1].travelTime);
        }

        #endregion

        #region Preview

        _previewT = GUILayout.HorizontalSlider(_previewT, 0, 1);
        if (GUILayout.Button("Preview Key")) { PreviewKey(_selectedIndex-1); }

        #endregion
        
        GUI.enabled = _clearEnabled;
        if (GUILayout.Button("Clear All")) { ClearKeys(); }
        GUI.enabled = true;

        if (EditorGUI.EndChangeCheck())
        {
            if (_selectedIndex > 0)
                SetPlatformPositionToKey(_selectedIndex-1);
        }
        serializedObject.ApplyModifiedProperties();
    }

    void AddKey()
    {
        Undo.RecordObject(_platform, "Added Key");
        _platform.platformKeys.Add(new MovingPlatformKey(_platform.transform, 0, 1));
        _selectedIndex++;
        UpdateButtonEnabling();
    }

    void InsertKey()
    {
        Undo.RecordObject(_platform, "Inserted Key");
        _platform.platformKeys.Insert(_selectedIndex, new MovingPlatformKey(_platform.transform, 0, 1));
        _selectedIndex++;
        UpdateButtonEnabling();
    }

    void RemoveKey()
    {
        Undo.RecordObject(_platform, "Removed Key");
        if (_platform.platformKeys.Count == 1)
        {
            ClearKeys();
            return;
        }
        
        _platform.platformKeys.RemoveAt(_selectedIndex - 1);
        if (_selectedIndex == _platform.platformKeys.Count + 1)
            _selectedIndex--;
    }

    void ClearKeys()
    {
        Undo.RecordObject(_platform, "Keys cleared");
        _selectedIndex = 0;

        if (_platform.platformKeys.Count != 0)
        {
            SetPlatformPositionToKey(0);
            _platform.platformKeys.Clear();
        }
        UpdateButtonEnabling();
    }

    void GoToPreviousKey()
    {
        _selectedIndex--;
        SetPlatformPositionToKey(_selectedIndex-1);
        UpdateButtonEnabling();
    }

    void GoToNextKey()
    {
        _selectedIndex++;
        SetPlatformPositionToKey(_selectedIndex-1);
        UpdateButtonEnabling();
    }

    void PreviewKey(int keyIndex)
    {
        _platform.MoveToKey(
            _platform.platformKeys[keyIndex].position,
            Quaternion.Euler(_platform.platformKeys[keyIndex].rotation),
            _platform.platformKeys[keyIndex].scale,
            _platform.platformKeys[keyIndex].travelTime
            );
    }

    void SetPlatformPositionToKey(int i)
    {
        _platform.transform.position = _platform.platformKeys[i].position;
        _platform.transform.rotation = Quaternion.Euler(_platform.platformKeys[i].rotation);
        _platform.transform.localScale = _platform.platformKeys[i].scale;
    }

    void UpdateButtonEnabling()
    {
        _previousKeyEnabled = _selectedIndex > 1;
        _nextKeyEnabled = _selectedIndex < _platform.platformKeys.Count;
        _clearEnabled = _platform.platformKeys.Count > 0;
        _removeEnabled = _selectedIndex != 0;
    }
}