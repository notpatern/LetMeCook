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
        public WinMenuUI winMenu;
        public LoseMenuUI loseMenu;
        public DialogUIManagement dialogUiManagement;

        public void LoadUI(LevelUIData levelUIData, DialogLevelData dialogLevelData)
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

            LoadDialogsPanel(levelUIData.dialogMenu, dialogLevelData);
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
            pauseMenu.Init(this);
        }

        void LoadDialogsPanel(GameObject prefab, DialogLevelData dialogLevelData)
        {
            GameObject go = Object.Instantiate(prefab, menuCanvas.transform);

            dialogUiManagement = go.GetComponent<DialogUIManagement>();
            dialogUiManagement.Init(dialogLevelData);
        }
    }
}