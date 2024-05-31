using Audio;
using ControlOptions;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] GameObject m_PanelContent;
    [SerializeField] Animator m_EndScreenAnimator;
    
    [Header("Stars")] 
    [SerializeField] TMP_Text m_ScoreTitle;
    [SerializeField] GameObject[] m_ActiveStarsGo;
    [SerializeField] float m_StarActivationTransitionTime = 0.5f;

    [Header("Score Texts")]
    [SerializeField] TMP_Text m_ScoreText;
    [SerializeField] TMP_Text m_CompletedRecipesRateText;
    [SerializeField] TMP_Text m_CompletedRecipes;
    [SerializeField] TMP_Text m_MissedRecipes;
    [SerializeField] TMP_Text m_BonusRecipes;
    [SerializeField] TMP_Text m_GroundedTime;

    [Header("Buttons")]
    [SerializeField] Button m_NextLevelButton;
    [SerializeField] Button m_GotoMainMenuButton;
    [SerializeField] Button m_RestartLevelButton;

    public void SetActive(bool state)
    {
        m_PanelContent.SetActive(state);
    }

    public void InitEndScreen(TempScoreContainer playerScore, LevelData nextLevelData, LevelData levelData)
    {
        m_EndScreenAnimator.SetTrigger("Start");

        ControlOptionsManagement.SetCursorIsPlayMode(false);
        ControlOptionsManagement.s_Instance.UpdateIsMainControlsActivated(false);

        UpdateHighScore(levelData, playerScore.m_Score);

        int completionRate = Mathf.RoundToInt(playerScore.m_CompletedRecipes / (float)playerScore.m_TotalRecipes * 100);

        SetScoreTitle(GetPlayerStarsNumber(playerScore.m_Score, playerScore.m_RequiredScore, completionRate));
        
        m_ScoreText.text = playerScore.m_Score + "pts (High Score : " + SaveSystem.GetSavedData().m_LevelHighScores[levelData.levelID] + ")";
        m_CompletedRecipesRateText.text = completionRate + "%" + " Completion Rate";
        m_CompletedRecipes.text = playerScore.m_CompletedRecipes + " Finished Recipes";
        m_MissedRecipes.text = (playerScore.m_TotalRecipes - playerScore.m_CompletedRecipes) + " Missed Recipes";
        m_BonusRecipes.text = (playerScore.m_BonusRecipes) + " Bonus Recipes";
        m_GroundedTime.text = playerScore.m_PlayerGroundedTime.ToString("0.#") + "s" + " Spend on the Ground";

        foreach (GameObject star in m_ActiveStarsGo)
        {
            star.SetActive(false);
        }

        SaveData data = SaveSystem.GetSavedData();

        if (!nextLevelData)
        {
            Destroy(m_NextLevelButton.gameObject);
        }
        else if (playerScore.m_RequiredScore > playerScore.m_Score && data.m_LevelReached <= levelData.levelID && playerScore.m_RequiredScore > data.m_LevelHighScores[levelData.levelID])
        {
            m_NextLevelButton.interactable = false;
        }
        else
        {
            StartCoroutine(ActiveStarWithOffsetTransition(completionRate));
            LevelIsWin(nextLevelData);
            m_NextLevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(nextLevelData.linkedScenePath));
        }

        m_GotoMainMenuButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(0));
        m_RestartLevelButton.onClick.AddListener(() => LevelLoader.s_instance.LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    void UpdateHighScore(LevelData levelData, int score)
    {
        int[] levelHighScores = SaveSystem.GetSavedData().m_LevelHighScores;

        if(levelHighScores == null)
        {
            levelHighScores = new int[levelData.levelID + 1];
        }
        else if(levelHighScores.Length < levelData.levelID + 1)
        {
            Array.Resize(ref levelHighScores, levelData.levelID + 1);
        }

        if (score > levelHighScores[levelData.levelID])
        {
            levelHighScores[levelData.levelID] = score;
        }

        SaveSystem.SaveLevelReached(levelHighScores);
    }

    void LevelIsWin(LevelData nextLevelData)
    {
        int reachedLevel = SaveSystem.GetSavedData().m_LevelReached;

        if (reachedLevel < nextLevelData.levelID)
        {
            SaveSystem.SaveLevelReached(nextLevelData.levelID);
        }
    }

    IEnumerator ActiveStarWithOffsetTransition(int completionRate)
    {
        m_ActiveStarsGo[0].SetActive(true);
        yield return new WaitForSeconds(m_StarActivationTransitionTime);

        if (completionRate >= 50)
        {
            m_ActiveStarsGo[1].SetActive(true);
            yield return new WaitForSeconds(m_StarActivationTransitionTime);
        }
        if (completionRate >= 100)
        {
            m_ActiveStarsGo[2].SetActive(true);
            yield return new WaitForSeconds(m_StarActivationTransitionTime);
        }
    }

    public bool[] GetUnlockedStars(int score, int maxMainRecipesFeedScore, int minimumRequiredScore, bool isMinimumRequeredScore)
    {
        bool[] result = new bool[GetStarsNumber()];

        if (!isMinimumRequeredScore) return result;

        for (int i = 0; i < result.Length; i++)
        {
            if (i / (float)(GetStarsNumber() - 1) <= score / (float)maxMainRecipesFeedScore)
            {
                result[i] = true;
            }
        }
        return result;
    }

    public int GetRequiredScoreUntilNextStar(int score, int maxMainRecipesFeedScore)
    {
        for (int i = 0; i < GetStarsNumber(); i++)
        {
            if(i / (float)(GetStarsNumber()-1) <= score / (float)maxMainRecipesFeedScore)
            {
                continue;
            }

            return Mathf.FloorToInt(i / (float)(GetStarsNumber() - 1) * maxMainRecipesFeedScore) - score;
        }

        return 0;
    }

    public int GetStarsNumber()
    {
        return m_ActiveStarsGo.Length;
    }

    public void OnButtonHover()
    {
        AudioManager.s_Instance.PlayOneShot2D(AudioManager.s_Instance.m_AudioSoundData.m_HoverUIButtons);
    }

    public int GetPlayerStarsNumber(int PlayerScore, int RequiredScore, int completionRate)
    {
        if (completionRate >= 100)
        {
            return 3;
        }
        if (completionRate >= 50)
        {
            return 2;
        }
        if (PlayerScore > RequiredScore)
        {
            return 1;
        }
        return 0;
    }
    
    public void SetScoreTitle(int starNumber)
    {
        switch (starNumber)
        {
            case 0:
                m_ScoreTitle.text = "You're so cooked...";
                break;
            case 1:
                m_ScoreTitle.text = "It's a good start";
                break;
            case 2:
                m_ScoreTitle.text = "Well done ! (get it?)";
                break;
            case 3:
                m_ScoreTitle.text = "Chef's kiss";
                break;
        }
    }
}

public class TempScoreContainer
{
    public int m_Score;
    public int m_TotalRecipes;
    public int m_CompletedRecipes;
    public int m_BonusRecipes;
    public int m_RequiredScore;
    public float m_PlayerGroundedTime;

    public TempScoreContainer(int score, int totalRecipes, int completedRecipes, int bonusRecipes, int requiredScore, float playerGroundedTime)
    {
        m_Score = score;
        m_TotalRecipes = totalRecipes;
        m_CompletedRecipes = completedRecipes;
        m_BonusRecipes = bonusRecipes;
        m_RequiredScore = requiredScore;
        m_PlayerGroundedTime = playerGroundedTime;
    }
}