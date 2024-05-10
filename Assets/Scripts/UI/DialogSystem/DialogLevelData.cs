using UnityEngine;
using System;
using FMODUnity;

namespace Dialog
{
    [CreateAssetMenu(fileName = "DialogLevelData", menuName = "LetMeCook/Dialog/DialogLevelData")]
    public class DialogLevelData : ScriptableObject
    {
        [Header("Parameters")]
        public float delayAfterDialog;
        public float delayAfterSentence;
        public int sentenceMaxLength;
        public int sentenceMinLength;
        public float letterDelay;

        [Header("Dialogs")]
        public DialogInfos[] dialogInfos;

        
    }

    [Serializable]
    public class DialogInfos
    {
        public string noGameContentNameID;
        public string pnjName;
        public string content;
        public EventReference audioVoice;

        [Header("Data loaded in DialogDataLoader Window Editor")]
        public LoadedContent loadedContent;

        [Serializable]
        public class LoadedContent
        {
            public string[] loadedString;
            public KeybindsData[] args;
        }
    }
}