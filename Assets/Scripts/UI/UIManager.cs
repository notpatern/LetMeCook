using System.Data;
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

        public void LoadUI(LevelUIData levelUIData)
        {
            LoadBaseCanvas(levelUIData.canvasPrefab);
            LoadPlayerHUDCanvas(levelUIData.playerHUBPrefab);

            LoadPauseMenu(levelUIData.pauseMenuPrefab);
            LoadOptionsMenu(levelUIData.optionMenu, levelUIData.isPauseMenuChild);
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
            GameObject go = isPauseMenuChild ? Object.Instantiate(prefab, pauseMenu.transform) : Object.Instantiate(prefab);
            go.SetActive(false);

            optionMenu = go.GetComponent<MENUScripts.OptionMenu>();
            pauseMenu.Init(this);
        }
    }
}