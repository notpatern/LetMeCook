using ControlOptions;
using TimeOption;
using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] GameObject m_PanelContent;
    [SerializeField] GameObject[] m_StarsGo;
    [SerializeField] TMP_Text m_ScoreText;
    [SerializeField] TMP_Text m_CompletedRecipesRateText;
    public void SetActive(bool state)
    {
        m_PanelContent.SetActive(state);
    }

    public void InitEndScreen(TempScoreContainer playerScore)
    {
        ControlOptionsManagement.SetCursorIsPlayMode(false);
        ControlOptionsManagement.s_Instance.DisableMainPlayerInputs();

        m_ScoreText.text = "Score : " + playerScore.m_Score;
        m_CompletedRecipesRateText.text = "Completed Recipes Rate : " + (playerScore.m_CompletedRecipes/(float)playerScore.m_TotalRecipes * 100) + "%";
    }
}

public class TempScoreContainer
{
    public int m_Score;
    public int m_TotalRecipes;
    public int m_CompletedRecipes;
    public int m_RequiredScore;

    public TempScoreContainer(int score, int totalRecipes, int completedRecipes, int requiredScore)
    {
        m_Score = score;
        m_TotalRecipes = totalRecipes;
        m_CompletedRecipes = completedRecipes;
        m_RequiredScore = requiredScore;
    }
}