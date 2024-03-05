using UnityEngine;
using System;

namespace Dialog
{
    [CreateAssetMenu(fileName = "DialogLevelData", menuName = "LetMeCook/Dialog/DialogLevelData")]
    public class DialogLevelData : ScriptableObject
    {
        [Header("Parameters")]
        public float sentenceDelay;
        public int sentenceMaxLength;
        public int sentenceMinLength;

        [Header("Dialogs")]
        public DialogInfos startLevel;
        public DialogInfos endLevelDialog;

        public DialogInfos[] startRecepieDialogs;
        public DialogInfos[] endRecepieDialogs;
    }

    [Serializable]
    public class DialogInfos
    {
        public string pnjName;
        public string content;
        [Header("Loaded data")]
        public bool isLoaded = false;
        public string[] loadedContent;
    }
}