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
        [SerializeField] GameEventScriptableObject m_CallDialogEvent;
        bool isInDialog;

        DialogLevelData m_DialogLevelData;
        Queue<DialogInfos> m_DialogInfosQueue = new Queue<DialogInfos>();

        const string c_ALPHACOLOR = "<color=#00000000>";

        public void Init(DialogLevelData dialogLevelData)
        {
            m_DialogLevelData = dialogLevelData;
            dialogPanel.SetActive(false);

            m_CallDialogEvent.BindEventAction(OnStartDialogEvent);
        }

        void OnStartDialogEvent(object args)
        {
            StartDialogQueue((DialogInfos)args);
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
            string displayText = "";
            int alphaIndex = 0;
            string defaultLoadedText;
            //Start dialog
            if (!isInDialog)
            {
                SetActiveDialog(true);
            }

            pnjNameDialogText.text = dialogInfos.pnjName;

            for (int i = 0; i < dialogInfos.loadedContent.Length; i++)
            {
                defaultLoadedText = dialogInfos.loadedContent[i];

                alphaIndex = 0;
                foreach (char c in dialogInfos.loadedContent[i])
                {
                    alphaIndex++;
                    displayText = defaultLoadedText.Insert(alphaIndex, c_ALPHACOLOR);
                    dialogText.text = displayText;

                    yield return new WaitForSeconds(m_DialogLevelData.letterDelay);
                }

                if (i + 1 < dialogInfos.loadedContent.Length)
                {
                    displayText = defaultLoadedText + "...";
                    dialogText.text = displayText;
                }


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