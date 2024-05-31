using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Audio;
using FMOD.Studio;
using PlayerSystems.PlayerInput;

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

        //FMOD UGLY
        EventInstance currentVoice;
        PLAYBACK_STATE currentPbState;
        //

        const string c_ALPHACOLOR = "<size=0><color=00000000>";
        public static string c_KEYOPTIONS = "<b><color=#FE4119FF>[";
        public static string c_KEYENDOPTIONS = "]</b></color>";

        bool isMusicPlaying = false;
        Coroutine currentDialog;

        public void Init(DialogLevelData dialogLevelData)
        {
            this.dialogLevelData = dialogLevelData;
            dialogPanel.SetActive(false);

            m_CallDialogEvent.BindEventAction(OnStartDialogEvent);
        }

        void Update()
        {
            if (isMusicPlaying && !isInDialog)
            {
                if(!isInDialog && dialogInfosQueue.Count > 0)
                {
                    currentVoice.stop(STOP_MODE.IMMEDIATE);
                }

                currentVoice.getPlaybackState(out currentPbState);
                if (currentPbState == PLAYBACK_STATE.STOPPED)
                {
                    isMusicPlaying = false;

                    if (dialogInfosQueue.Count > 0)
                    {
                        StartCoroutine(StartDialog(false));
                    }
                    else
                    {
                        SetActiveDialog(false);
                    }
                }
            }
        }

        void OnStartDialogEvent(object args)
        {
            StartDialogQueue((DialogInfos)args, true);
        }

        public bool StartDialogQueue(DialogInfos dialogInfos, bool overrideLastDialog)
        {
            dialogInfosQueue.Enqueue(dialogInfos);
           
            if (!overrideLastDialog && isInDialog) return false;

            if(currentDialog != null)
            {
                StopCoroutine(currentDialog);
            }

            currentDialog = StartCoroutine(StartDialog(overrideLastDialog));

            return true;
        }

        IEnumerator StartDialog(bool overrideLastDialog)
        {
            DialogInfos dialogInfos = dialogInfosQueue.Dequeue();
            //string displayText = "";
            //int alphaIndex = 0;
            //string defaultLoadedText;
            //Start dialog
            if (!isInDialog)
            {
                // SetActiveDialog(true);
            }

            pnjNameDialogText.text = dialogInfos.pnjName;
            //InputAction inputActionArg;
            //KeybindsData keybindsData;
            string[] loadedKeys = new string[dialogInfos.loadedContent.args.Length];

            if(isMusicPlaying && overrideLastDialog)
            {
                currentVoice.stop(STOP_MODE.IMMEDIATE);
            }

            if (!dialogInfos.audioVoice.IsNull)
            {
                currentVoice = AudioManager.s_Instance.CreateInstance(dialogInfos.audioVoice);
                currentVoice.start();
                currentVoice.release();
                isMusicPlaying = true;
            }
            else
            {
                isMusicPlaying = false;
            }

           // if (dialogInfos.loadedContent.args.Length > 0)
           // {
           //     for (int j = 0; j < dialogInfos.loadedContent.args.Length; j++)
           //     {
           //         keybindsData = dialogInfos.loadedContent.args[j];
           //         inputActionArg = InputManager.s_PlayerInput.asset.FindAction(keybindsData.inputActionReference.action.id);
           //         loadedKeys[j] = c_KEYOPTIONS + inputActionArg.GetBindingDisplayString(keybindsData.bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames) + c_KEYENDOPTIONS;
           //     }
           // }
           //
           //for (int i = 0; i < dialogInfos.loadedContent.loadedString.Length; i++)
           //{
           //    defaultLoadedText = dialogInfos.loadedContent.loadedString[i];
           //    
           //    if (dialogInfos.loadedContent.args.Length > 0)
           //    {
           //        defaultLoadedText = string.Format(defaultLoadedText, loadedKeys);
           //    }
           //    else
           //    {
           //        defaultLoadedText = dialogInfos.loadedContent.loadedString[i];
           //    }
           //
           //    alphaIndex = 0;
           //    bool isBaliseOpended = false;
           //    foreach (char c in defaultLoadedText)
           //    {
           //        int offset = ReturnBaliseOffset(ref isBaliseOpended, defaultLoadedText, alphaIndex);
           //        
           //        alphaIndex += offset;
           //
           //        if(alphaIndex > defaultLoadedText.Length)
           //        {
           //            alphaIndex = defaultLoadedText.Length;
           //        }
           //
           //        displayText = defaultLoadedText.Insert(alphaIndex, c_ALPHACOLOR);
           //        dialogText.text = displayText;
           //
           //        yield return new WaitForSeconds(dialogLevelData.letterDelay);
           //    }
           //
           //    if (i + 1 < dialogInfos.loadedContent.loadedString.Length)
           //    {
           //        dialogText.text = defaultLoadedText + "...";
           //    }
           //

            //    yield return new WaitForSeconds(dialogLevelData.delayAfterSentence);
            //}

            yield return new WaitForSeconds(dialogLevelData.delayAfterDialog);

            //Stop dialog
            if (dialogInfosQueue.Count == 0 && !isMusicPlaying)
            {
                SetActiveDialog(false);
            }
            else if(!isMusicPlaying)
            {
                //continue dialogs queue
                StartCoroutine(StartDialog(overrideLastDialog));
            }
            else
            {
                isInDialog = false;
            }
        }

        int ReturnBaliseOffset(ref bool isBaliseOpended, string defaultLoadedText, int alphaIndex)
        {
            if (isBaliseOpended)
            {
                if (CheckForBaliseAndReturnOffset(defaultLoadedText, c_KEYENDOPTIONS, alphaIndex))
                {
                    isBaliseOpended = false;
                    return c_KEYENDOPTIONS.Length;
                }
            }
            else
            {
                if (CheckForBaliseAndReturnOffset(defaultLoadedText, c_KEYOPTIONS, alphaIndex))
                {
                    isBaliseOpended = true;
                    return c_KEYOPTIONS.Length;
                }
            }

            return 1;
        }

        bool CheckForBaliseAndReturnOffset(string data, string balise, int alphaIndex)
        {
            if (data.Length - alphaIndex >= balise.Length && data[alphaIndex] == balise[0])
            {
                for (int j = 0; j < balise.Length; j++)
                {
                    if (data[alphaIndex + j] != balise[j])
                    {
                        return false;
                    }

                }

                return true;
            }

            return false;
        }

        void SetActiveDialog(bool state)
        {
            dialogPanel.SetActive(state);
            isInDialog = state;
        }
    }
}