using Dialog;
using Manager;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(DialogInfosTriggerZone), true)]
public class DialogTriggerEditor : Editor
{
    DialogLevelData m_DialogLevelData;
    int index = 0;
    DialogInfos[] loadedDialogInfos;

    DialogInfos[][] loadedRandomDialogInfos;
    FieldInfo[] loadedRandomFieldInfos;

    public override void OnInspectorGUI()
    {
        DialogInfosTriggerZone picker = target as DialogInfosTriggerZone;

        if (!m_DialogLevelData)
        {
            LevelManager levelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;
            m_DialogLevelData = levelManager.m_LevelData.dialogLevelData;
        }

        if (!m_DialogLevelData) return;

        index = EditorGUILayout.Popup(
                "DialogInfos:",
                index,
                LoadDialogInfosOptions()
            );

        LayerMask tempMask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(picker.triggerableLayers), InternalEditorUtility.layers);
        picker.triggerableLayers = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

        picker.destroyOnTrigger = EditorGUILayout.Toggle("Destroy On Trigger", picker.destroyOnTrigger);

        if (index >= m_DialogLevelData.dialogInfos.Length)
        {
            index = m_DialogLevelData.dialogInfos.Length-1;
        }

        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;

        
        picker.m_CurrentDialogInfos = m_DialogLevelData.dialogInfos[index];
        picker.m_CallDialogEvent = EditorGUILayout.ObjectField("Call Dialog Event", picker.m_CallDialogEvent, typeof(GameEventScriptableObject), false) as GameEventScriptableObject;
    }


    string[] LoadDialogInfosOptions()
    {
        FieldInfo[] fieldInfos = typeof(DialogLevelData).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
        loadedDialogInfos = new DialogInfos[fieldInfos.Length];

        List<string> options = new List<string>();
        int fieldInfosId = -1;
        int filedsInfosId = -1;
        int dialogArrayId = 0;
        foreach (FieldInfo fieldInfo in fieldInfos)
        {

            if (fieldInfo.FieldType == typeof(DialogInfos))
            {
                fieldInfosId++;
                CheckResizeLoadedDialogInfos(fieldInfosId);
                loadedDialogInfos[fieldInfosId] = fieldInfo.GetValue(m_DialogLevelData) as DialogInfos;

                options.Add(fieldInfo.Name);
            }
            else if (fieldInfo.FieldType == typeof(DialogInfos[]))
            {
                filedsInfosId++;
                Array.Resize(ref loadedRandomDialogInfos, filedsInfosId + 1);
                Array.Resize(ref loadedRandomFieldInfos, filedsInfosId + 1);
                loadedRandomDialogInfos[filedsInfosId] = fieldInfo.GetValue(m_DialogLevelData) as DialogInfos[];
                loadedRandomFieldInfos[filedsInfosId] = fieldInfo;

                if (loadedRandomDialogInfos[filedsInfosId].Length == 0) continue;
                dialogArrayId = 0;
                foreach (DialogInfos dialogInfo in loadedRandomDialogInfos[filedsInfosId])
                {
                    dialogArrayId++;
                    options.Add(fieldInfo.Name + " | " + dialogInfo.noGameContentNameID);

                    fieldInfosId++;
                    CheckResizeLoadedDialogInfos(fieldInfosId);
                    loadedDialogInfos[fieldInfosId] = dialogInfo;
                }
            }

        }

        return options.ToArray();
    }

    void CheckResizeLoadedDialogInfos(int size)
    {
        if (size >= loadedDialogInfos.Length)
        {
            Array.Resize(ref loadedDialogInfos, size + 1);
        }
    }
}