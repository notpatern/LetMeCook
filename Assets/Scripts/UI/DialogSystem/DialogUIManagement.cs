using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Audio;
using FMOD.Studio;
using System;

namespace Dialog
{
    public class DialogUIManagement : MonoBehaviour
    {
        [SerializeField] GameObject dialogPanel;
        [SerializeField] TMP_Text dialogText;
        [SerializeField] TMP_Text pnjNameDialogText;
        [SerializeField] GameEventScriptableObject m_CallDialogEvent;
        bool isInDialog;

        DialogLevelData dialogLevelData;
        Queue<DialogInfos> dialogInfosQueue = new Queue<DialogInfos>();

        EventInstance currentVoice;

        const string c_ALPHACOLOR = "<color=#00000000>";

        bool isMusicPlaying = false;

        public void Init(DialogLevelData dialogLevelData)
        {
            this.dialogLevelData = dialogLevelData;
            dialogPanel.SetActive(false);

            m_CallDialogEvent.BindEventAction(OnStartDialogEvent);
        }

        void OnStartDialogEvent(object args)
        {
            StartDialogQueue((DialogInfos)args);
        }

        public bool StartDialogQueue(DialogInfos dialogInfos)

        {
            dialogInfosQueue.Enqueue(dialogInfos);

            if (isInDialog || isMusicPlaying) return false;

            StartCoroutine(StartDialog());

            return true;
        }

        IEnumerator StartDialog()
        {
            DialogInfos dialogInfos = dialogInfosQueue.Dequeue();
            string displayText = "";
            int alphaIndex = 0;
            string defaultLoadedText;
            //Start dialog
            if (!isInDialog)
            {
                SetActiveDialog(true);
            }

            pnjNameDialogText.text = dialogInfos.pnjName;
            InputAction inputActionArg;
            KeybindsData keybindsData;
            string[] loadedKeys = new string[dialogInfos.loadedContent.args.Length];

            currentVoice = AudioManager.s_Instance.CreateInstance(dialogInfos.audioVoice);
            currentVoice.setCallback(DialogVoiceMarkerEventCallback, EVENT_CALLBACK_TYPE.STOPPED);
            currentVoice.start();

            if (dialogInfos.loadedContent.args.Length > 0)
            {
                loadedKeys = new string[dialogInfos.loadedContent.args.Length];
                for (int j = 0; j < dialogInfos.loadedContent.args.Length; j++)
                {
                    keybindsData = dialogInfos.loadedContent.args[j];
                    inputActionArg = keybindsData.inputActionReference;
                    loadedKeys[j] = inputActionArg.GetBindingDisplayString(keybindsData.bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
                }
            }

            for (int i = 0; i < dialogInfos.loadedContent.loadedString.Length; i++)
            {
                defaultLoadedText = dialogInfos.loadedContent.loadedString[i];
                
                if (dialogInfos.loadedContent.args.Length > 0)
                {
                    defaultLoadedText = string.Format(defaultLoadedText, loadedKeys);
                }
                else
                {
                    defaultLoadedText = dialogInfos.loadedContent.loadedString[i];
                }

                alphaIndex = 0;
                foreach (char c in defaultLoadedText)
                {
                    alphaIndex++;
                    displayText = defaultLoadedText.Insert(alphaIndex, c_ALPHACOLOR);
                    dialogText.text = displayText;

                    yield return new WaitForSeconds(dialogLevelData.letterDelay);
                }

                if (i + 1 < dialogInfos.loadedContent.loadedString.Length)
                {
                    displayText = defaultLoadedText + "...";
                    dialogText.text = displayText;
                }


                yield return new WaitForSeconds(dialogLevelData.sentenceDelay);
            }

            //Stop dialog
            if (dialogInfosQueue.Count == 0)
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

        FMOD.RESULT DialogVoiceMarkerEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
        {
            if (type == EVENT_CALLBACK_TYPE.STOPPED)
            {
                isMusicPlaying = false;
                AudioManager.s_Instance.CleanUp(currentVoice);
            }

            return FMOD.RESULT.OK;
        }
    }
}