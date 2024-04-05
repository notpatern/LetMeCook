using ControlOptions;
using System.Collections;
using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] GameObject m_PanelContent;
    [Header("Stars")]
    [SerializeField] GameObject[] m_ActiveStarsGo;
    [SerializeField] float m_StarActivationTransitionTime = 0.5f;

    [Header("Score Texts")]
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

        int minimumRequiredScoreOverflow = playerScore.m_Score - playerScore.m_RequiredScore;

        foreach(GameObject star in m_ActiveStarsGo)
        {
            star.SetActive(false);
        }

        if(minimumRequiredScoreOverflow < 0) 
        {
            return;
        }

        //stars are generated based on *2 minimum required score is the max for now
        float scoreStep = playerScore.m_RequiredScore / (float)m_ActiveStarsGo.Length;
        StartCoroutine(ActiveStarWithOffsetTransition(minimumRequiredScoreOverflow, scoreStep));
    }

    IEnumerator ActiveStarWithOffsetTransition(int minimumRequiredScoreOverflow, float scoreStep)
    {
        for (int i = 0; i < m_ActiveStarsGo.Length; i++)
        {
            if (i == 0 || minimumRequiredScoreOverflow >= scoreStep * i)
            {
                m_ActiveStarsGo[i].SetActive(true);
                yield return new WaitForSeconds(m_StarActivationTransitionTime);
            }
            else
            {
                break;
            }
        }
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