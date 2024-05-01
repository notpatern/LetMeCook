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
        [Header("Data loaded in DialogDataLoader Window Editor")]
        public string[] loadedContent;
    }
}