using Dialog;
using UnityEngine;

namespace UI
{
    public class UIManager
    {
        GameObject menuCanvas;
        public MENUScripts.PauseMenuUI pauseMenu;
        public MENUScripts.OptionMenu optionMenu;
        public PlayerHUD playerHUD;
        public EndConditionUI endConditionUI;
        public EndScreenUI endScreen;
        public DialogUIManagement dialogUiManagement;

        //if there are problems with multiple layers then do a layer system with an UIContent parent or something and add a layer parameter
        public void LoadUI(LevelUIData levelUIData, DialogLevelData dialogLevelData, Transform endConditionParent)
        {
            LoadBaseCanvas(levelUIData.canvasPrefab);

            if (levelUIData.playerHUBPrefab)
            {
                LoadPlayerHUDCanvas(levelUIData.playerHUBPrefab);
            }

            if (levelUIData.pauseMenuPrefab)
            {
                LoadPauseMenu(levelUIData.pauseMenuPrefab);
            }

            LoadOptionsMenu(levelUIData.optionMenu, levelUIData.isPauseMenuChild);

            if (levelUIData.dialogMenu)
            {
                LoadDialogsPanel(levelUIData.dialogMenu, dialogLevelData);
            }

            if(levelUIData.endConditionPrefab && endConditionParent)
            {
                LoadEndConditionUI(levelUIData.endConditionPrefab, endConditionParent);
            }

            if(levelUIData.endScreenMenuPrefab)
            {
                LoadEndScreen(levelUIData.endScreenMenuPrefab);
            }
        }

        void LoadEndScreen(GameObject endScreenPrefab)
        {
            endScreen = Object.Instantiate(endScreenPrefab, menuCanvas.transform).GetComponent<EndScreenUI>();
            endScreen.SetActive(false);
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
            GameObject go = isPauseMenuChild ? Object.Instantiate(prefab, pauseMenu.transform) : Object.Instantiate(prefab, menuCanvas.transform);
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