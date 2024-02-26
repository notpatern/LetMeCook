using UnityEngine;

namespace UI
{
    public class UIManager
    {
        GameObject menuCanvas;
        public MENUScripts.PauseMenuUI pauseMenu;
        public WinMenuUI winMenu;
        public LoseMenuUI loseMenu;
        public PlayerHUD playerHUD;

        public void LoadUI(LevelUIData levelUIData)
        {
            LoadBaseCanvas(levelUIData.canvasPrefab);
            LoadPlayerHUDCanvas(levelUIData.canvasPrefab);

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

        void LoadWinMenu(GameObject prefab) //Load win menu canvas
        {
            winMenu = Object.Instantiate(prefab, playerHUD.transform).GetComponent<WinMenuUI>();
        }

        void LoadLoseMenu(GameObject prefab) //Load lose menu canvas
        {
            loseMenu = Object.Instantiate(prefab, playerHUD.transform).GetComponent<LoseMenuUI>();
        }
    }
}