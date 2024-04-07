using ControlOptions;
using System.Collections;
using TimeOption;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] GameObject m_PanelContent;
    [SerializeField] Animator m_EndScreenAnimator;
    [Header("Stars")]
    [SerializeField] GameObject[] m_ActiveStarsGo;
    [SerializeField] float m_StarActivationTransitionTime = 0.5f;

    [Header("Score Texts")]
    [SerializeField] TMP_Text m_ScoreText;
    [SerializeField] TMP_Text m_CompletedRecipesRateText;
    [SerializeField] TMP_Text m_CompletedRecipes;
    [SerializeField] TMP_Text m_MissedRecipes;
    [SerializeField] TMP_Text m_GroundedTime;

    [Header("Buttons")]
    [SerializeField] Button m_NextLevelButton;
    [SerializeField] Button m_GotoMainMenuButton;
    [SerializeField] Button m_RestartLevelButton;

    public void SetActive(bool state)
    {
        m_PanelContent.SetActive(state);
    }

    public void InitEndScreen(TempScoreContainer playerScore, LevelData nextLevelData)
    {
        m_EndScreenAnimator.SetTrigger("Start");

        ControlOptionsManagement.SetCursorIsPlayMode(false);
        ControlOptionsManagement.s_Instance.DisableMainPlayerInputs();

        m_ScoreText.text = "Score : " + playerScore.m_Score;
        m_CompletedRecipesRateText.text = "Completed Recipes Rate : " + (playerScore.m_CompletedRecipes/(float)playerScore.m_TotalRecipes * 100) + "%";
        m_CompletedRecipes.text = "Completed Reciped : " + playerScore.m_CompletedRecipes;
        m_MissedRecipes.text = "Missed Reciped : " + playerScore.m_TotalRecipes;
        m_GroundedTime.text = "Time On Ground : 15s";//TODO link the grounded time

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

        if(!nextLevelData)
        {
            Destroy(m_NextLevelButton.gameObject);
        }
        else if(playerScore.m_RequiredScore > playerScore.m_Score)
        {
            m_NextLevelButton.interactable = false;
        }
        else
        {
            LevelIsWin();
            m_NextLevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(nextLevelData.linkedScenePath));
        }

        m_GotoMainMenuButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(0));
        m_RestartLevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    void LevelIsWin()
    {
        int reachedLevel = SaveSystem.GetLevelReached();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (reachedLevel < currentSceneIndex + 1)
        {
            SaveSystem.SaveLevelReached(currentSceneIndex + 1);
        }
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