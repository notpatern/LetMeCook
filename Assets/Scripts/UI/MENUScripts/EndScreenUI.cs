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

    TempScoreContainer score;

    public void SetActive(bool state)
    {
        m_PanelContent.SetActive(state);
    }

    public void InitEndScreen(TempScoreContainer playerScore, LevelData nextLevelData)
    {
        score = playerScore;
        m_EndScreenAnimator.SetTrigger("Start");

        ControlOptionsManagement.SetCursorIsPlayMode(false);
        ControlOptionsManagement.s_Instance.UpdateIsMainControlsActivated(false);

        m_ScoreText.text = playerScore.m_Score + "pts";
        m_CompletedRecipesRateText.text = Mathf.RoundToInt(playerScore.m_CompletedRecipes / (float)playerScore.m_TotalRecipes * 100) + "%" + " Completion Rate";
        m_CompletedRecipes.text = playerScore.m_CompletedRecipes + " Finished Recipes";
        m_MissedRecipes.text = (playerScore.m_TotalRecipes - playerScore.m_CompletedRecipes) + " Missed Recipes";
        m_GroundedTime.text = playerScore.m_PlayerGroundedTime.ToString("0.#") + "s" + " Spend on the Ground";

        foreach (GameObject star in m_ActiveStarsGo)
        {
            star.SetActive(false);
        }

        if (!nextLevelData)
        {
            Destroy(m_NextLevelButton.gameObject);
        }
        else if (playerScore.m_RequiredScore > playerScore.m_Score)
        {
            m_NextLevelButton.interactable = false;
        }
        else
        {
            StartCoroutine(ActiveStarWithOffsetTransition(playerScore.m_RequiredScore));
            LevelIsWin(nextLevelData);
            m_NextLevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(nextLevelData.linkedScenePath));
        }

        m_GotoMainMenuButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(0));
        m_RestartLevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    void LevelIsWin(LevelData nextLevelData)
    {
        int reachedLevel = SaveSystem.GetLevelReached();

        if (reachedLevel < nextLevelData.levelID)
        {
            SaveSystem.SaveLevelReached(nextLevelData.levelID);
        }
    }

    IEnumerator ActiveStarWithOffsetTransition(int minimumRequiredScoreOverflow)
    {
        
        m_ActiveStarsGo[0].SetActive(true);
        yield return new WaitForSeconds(m_StarActivationTransitionTime);

        if (score.m_Score >= minimumRequiredScoreOverflow * 2)
        {
            m_ActiveStarsGo[1].SetActive(true);
            yield return new WaitForSeconds(m_StarActivationTransitionTime);
        }
        if (score.m_CompletedRecipes == (float)score.m_TotalRecipes)
        {
            m_ActiveStarsGo[2].SetActive(true);
            yield return new WaitForSeconds(m_StarActivationTransitionTime);
        }
    }
}

public class TempScoreContainer
{
    public int m_Score;
    public int m_TotalRecipes;
    public int m_CompletedRecipes;
    public int m_RequiredScore;
    public float m_PlayerGroundedTime;

    public TempScoreContainer(int score, int totalRecipes, int completedRecipes, int requiredScore, float playerGroundedTime)
    {
        m_Score = score;
        m_TotalRecipes = totalRecipes;
        m_CompletedRecipes = completedRecipes;
        m_RequiredScore = requiredScore;
        m_PlayerGroundedTime = playerGroundedTime;
    }
}