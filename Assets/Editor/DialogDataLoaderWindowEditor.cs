using UnityEditor;
using UnityEngine;
using Dialog;
using System.Collections.Generic;
using System;
using System.Reflection;

public class DialogDataLoaderWindowEditor : EditorWindow
{
    DialogLevelData m_DialogLevelData;
    int index = 0;
    DialogInfos[] loadedDialogInfos; 

    [MenuItem("Window/Dialog Loader")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DialogDataLoaderWindowEditor));
    }

    private void OnGUI()
    {
        m_DialogLevelData = EditorGUILayout.ObjectField(m_DialogLevelData, typeof(DialogLevelData), false, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as DialogLevelData;

        if (!m_DialogLevelData) return;

        index = EditorGUILayout.Popup(
                "Component:",
                index,
                LoadDialogInfosOptions()
            );


        EditorGUILayout.LabelField("DialogContent : ");

        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;
        loadedDialogInfos[index].content = EditorGUILayout.TextArea(loadedDialogInfos[index].content);

        if (GUILayout.Button("LoadDialog"))
        {
            LoadDialogInfos(ref loadedDialogInfos[index].loadedContent);
        }
    }

    string[] LoadDialogInfosOptions()
    {
        FieldInfo[] fieldInfos = typeof(DialogLevelData).GetFields(BindingFlags.Instance | BindingFlags.Public);
        loadedDialogInfos = new DialogInfos[fieldInfos.Length];
        List<string> options = new List<string>();
        foreach(FieldInfo fieldInfo in fieldInfos)
        {
            if(fieldInfo.FieldType == typeof(DialogInfos))
            {
                options.Add(fieldInfo.Name);

                loadedDialogInfos[options.Count - 1] = fieldInfo.GetValue(m_DialogLevelData) as DialogInfos;
            }
        }

        return options.ToArray();
    }


    void LoadDialogInfos(ref string[] result)
    {
        result = new string[Mathf.CeilToInt(loadedDialogInfos[index].content.Length / m_DialogLevelData.sentenceMaxLength) + 1];
        //Split content in result
        int id = 0;
        foreach (string data in ChunksUpto(loadedDialogInfos[index].content, m_DialogLevelData.sentenceMaxLength))
        {
            result[id] = data;

            id++;
        }

        //Check if sentences are fiting with the min length then resize
        if (result.Length > 1 && result[result.Length - 1].Length < m_DialogLevelData.sentenceMinLength)
        {
            result[result.Length - 2] += result[result.Length - 1];
            Array.Resize(ref result, result.Length - 1);
        }
    }

    //Get sentences split by size and char ' ' for space and complete senteces 
    IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
    {
        int currentChunckSize = 0;
        int maxChunkSizeOffset = maxChunkSize;
        for (int i = 0; i < str.Length; i += maxChunkSizeOffset)
        {
            maxChunkSizeOffset = maxChunkSize;
            currentChunckSize = Mathf.Min(currentChunckSize + maxChunkSizeOffset, str.Length - 1);

            while (i > 0 && str[i] != ' ')
            {
                i--;
            }

            while (currentChunckSize > 0 && currentChunckSize != str.Length - 1 && str[currentChunckSize] != ' ')
            {
                currentChunckSize--;
                maxChunkSizeOffset--;
            }

            yield return str.Substring(i, Mathf.Min(maxChunkSizeOffset, str.Length - i));
        }
    }
}
