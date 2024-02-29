using UnityEngine;
using TMPro;

namespace Manager
{
    public class EndGameMenu : LevelManager
    {
        [SerializeField] TMP_Text m_ScoreText;

        protected override void Awake()
        {
            m_ScoreText.text = "Score : " + LevelScoreDataTransmetor.s_Instance.score;
        }
    }
}