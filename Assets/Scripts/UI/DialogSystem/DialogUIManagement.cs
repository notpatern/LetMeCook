using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace Dialog
{
    public class DialogUIManagement : MonoBehaviour
    {
        [SerializeField] GameObject dialogPanel;
        [SerializeField] TMP_Text dialogText;
        [SerializeField] TMP_Text pnjNameDialogText;
        bool isInDialog;

        DialogLevelData m_DialogLevelData;
        Queue<DialogInfos> m_DialogInfosQueue = new Queue<DialogInfos>();


        public void Init(DialogLevelData dialogLevelData)
        {
            m_DialogLevelData = dialogLevelData;
            dialogPanel.SetActive(false);
            StartDialogQueue(dialogLevelData.startLevel);
        }

        public bool StartDialogQueue(DialogInfos dialogInfos)
        {
            m_DialogInfosQueue.Enqueue(dialogInfos);

            if (isInDialog) return false;

            StartCoroutine(StartDialog());

            return true;
        }

        IEnumerator StartDialog()
        {
            DialogInfos dialogInfos = m_DialogInfosQueue.Dequeue();

            if (!dialogInfos.isLoaded)
            {
                LoadDialogInfos(dialogInfos, ref dialogInfos.loadedContent);
            }

            //Start dialog
            if (!isInDialog)
            {
                SetActiveDialog(true);
            }

            pnjNameDialogText.text = dialogInfos.pnjName;

            for (int i = 0; i < dialogInfos.loadedContent.Length; i++)
            {
                dialogText.text = dialogInfos.loadedContent[i];

                yield return new WaitForSeconds(m_DialogLevelData.sentenceDelay);
            }

            //Stop dialog
            if (m_DialogInfosQueue.Count == 0)
            {
                SetActiveDialog(false);
            }
            else
            {
                //continue dialogs queue
                StartCoroutine(StartDialog());
            }
        }

        void SetActiveDialog(bool state)
        {
            dialogPanel.SetActive(state);
            isInDialog = state;
        }

        void LoadDialogInfos(DialogInfos dialogInfos, ref string[] result)
        {
            result = new string[Mathf.CeilToInt(dialogInfos.content.Length / m_DialogLevelData.sentenceMaxLength)+1];
            //Split content in result
            int id = 0;
            foreach (string data in ChunksUpto(dialogInfos.content, m_DialogLevelData.sentenceMaxLength))
            {
                result[id] = data;

                id++;
            }

            //Check if sentences are fiting with the min length then resize
            if(result.Length > 1 && result[result.Length-1].Length < m_DialogLevelData.sentenceMinLength)
            {
                result[result.Length - 2] += result[result.Length - 1];
                Array.Resize(ref result, result.Length - 1);
            }

            dialogInfos.isLoaded = true;
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

                while (i>0 && str[i] != ' ') 
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
}