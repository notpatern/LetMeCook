using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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

            //StartCoroutine(StartDialog());

            return true;
        }



        IEnumerator StartDialog()
        {
            DialogInfos dialogInfos = m_DialogInfosQueue.Dequeue();

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
    }
}