using UnityEngine;

namespace UI
{
    public class UIManager
    {
        GameObject menuCanvas;
        public MENUScripts.PauseMenuUI pauseMenu;
        public PlayerHUD playerHUD;
        public WinMenuUI winMenu;
        public LoseMenuUI loseMenu;

        public void LoadUI(LevelUIData levelUIData)
        {
            LoadBaseCanvas(levelUIData.canvasPrefab);
            LoadPlayerHUDCanvas(levelUIData.playerHUBPrefab);

            LoadPauseMenu(levelUIData.pauseMenuPrefab);
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
            pauseMenu = Object.Instantiate(prefab, menuCanvas.transform).GetComponent<MENUScripts.PauseMenuUI>();
        }
    }
}