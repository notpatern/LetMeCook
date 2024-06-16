using Dialog;
using RecipeSystem.Core;
using UnityEngine;
using TMPro;

namespace UI
{
    public class UIManager
    {
        GameObject menuCanvas;
        public MENUScripts.PauseMenuUI pauseMenu;
        public MENUScripts.OptionMenu optionMenu;
        public PlayerHUD playerHUD;
        public EndConditionUI endConditionUI;
        public PlayerWarningText playerWarningText;
        public EndScreenUI endScreen;
        public DialogUIManagement dialogUiManagement;

        public RecipeUI recipeUI;
        TMP_Text scoreText;

        //if there are problems with multiple layers then do a layer system with an UIContent parent or something and add a layer parameter
        public void LoadUI(LevelUIData levelUIData, DialogLevelData dialogLevelData, Transform m_EndconditionWorldParentUI, Transform m_EndconditionPlayerParentUI, Transform m_ScoreWorldUIParent)
        {
            LoadBaseCanvas(levelUIData.canvasPrefab);

            if (levelUIData.playerHUBPrefab)
            {
                LoadPlayerHUDCanvas(levelUIData.playerHUBPrefab);
            }

            if (levelUIData.dialogMenu)
            {
                LoadDialogsPanel(levelUIData.dialogMenu, dialogLevelData);
            }

            if(levelUIData.endConditionPrefab && levelUIData.playerWarningText && m_EndconditionPlayerParentUI && m_EndconditionWorldParentUI)
            {
                LoadPlayerWarningText(levelUIData.playerWarningText, m_EndconditionPlayerParentUI);
                LoadEndConditionUI(levelUIData.endConditionPrefab, m_EndconditionWorldParentUI);
            }

            if(m_ScoreWorldUIParent)
            {
                LoadScoreWorldUI(levelUIData.scorePrefab, m_ScoreWorldUIParent);
            }

            if (levelUIData.recipeContentParent)
            {
                LoadRecipeContentParent(levelUIData.recipeContentParent);
            }

            if (levelUIData.pauseMenuPrefab)
            {
                LoadPauseMenu(levelUIData.pauseMenuPrefab);
            }

            LoadOptionsMenu(levelUIData.optionMenu, levelUIData.isPauseMenuChild);

            if (levelUIData.endScreenMenuPrefab)
            {
                LoadEndScreen(levelUIData.endScreenMenuPrefab);
            }
        }

        void LoadRecipeContentParent(GameObject recipieEndMenu)
        {
            recipeUI = Object.Instantiate(recipieEndMenu, menuCanvas.transform).GetComponent<RecipeUI>();
        }

        void LoadEndScreen(GameObject endScreenPrefab)
        {
            endScreen = Object.Instantiate(endScreenPrefab, menuCanvas.transform).GetComponent<EndScreenUI>();
            endScreen.SetActive(false);
        }

        void LoadScoreWorldUI(GameObject scorePrefab, Transform parent)
        {
            scoreText = Object.Instantiate(scorePrefab, parent).GetComponent<TMP_Text>();
            UpdateScore(0, null, 0);
        }

        public void UpdateScore(int amount, bool[] scoreUnlocked, int scoreUntilNextStar)
        {
            scoreText.text = amount.ToString();

            if (playerHUD)
            {
                playerHUD.UpdateScore(scoreUnlocked, scoreUntilNextStar);
            }
        }

        void LoadPlayerWarningText(GameObject playerWarningTextPrefab, Transform parent) 
        {
            playerWarningText = Object.Instantiate(playerWarningTextPrefab, parent).GetComponent<PlayerWarningText>();
        }

        void LoadEndConditionUI(GameObject endConditionPrefab, Transform parent)
        {
            endConditionUI = Object.Instantiate(endConditionPrefab, parent).GetComponent<EndConditionUI>();
        }

        void LoadBaseCanvas(GameObject canvasPrefab) //Load canvas for all the menus
        {
            menuCanvas = Object.Instantiate(canvasPrefab);
        }

        void LoadPlayerHUDCanvas(GameObject canvasPrefab) //Load canvas for all the in game HUD
        {
            playerHUD = Object.Instantiate(canvasPrefab).GetComponent<PlayerHUD>();
        }

        void LoadPauseMenu(GameObject prefab) //Load pause menu canvas 
        {
            GameObject pauseMenuGO = Object.Instantiate(prefab, menuCanvas.transform);
            pauseMenuGO.SetActive(false);
            pauseMenu = pauseMenuGO.GetComponent<MENUScripts.PauseMenuUI>();
        }

        void LoadOptionsMenu(GameObject prefab, bool isPauseMenuChild)
        {
            GameObject go = isPauseMenuChild ? Object.Instantiate(prefab, pauseMenu.optionMenuParent) : Object.Instantiate(prefab, menuCanvas.transform);
            go.SetActive(false);

            optionMenu = go.GetComponent<MENUScripts.OptionMenu>();

            if (pauseMenu)
            {
                pauseMenu.Init(this);
            }
        }

        void LoadDialogsPanel(GameObject prefab, DialogLevelData dialogLevelData)
        {
            GameObject go = Object.Instantiate(prefab, menuCanvas.transform);

            dialogUiManagement = go.GetComponent<DialogUIManagement>();
            dialogUiManagement.Init(dialogLevelData);
        }
    }
}