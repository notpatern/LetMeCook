using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public MENUScripts.PauseMenuUI pauseMenu;
        public WinMenuUI winMenu;
        public LoseMenuUI loseMenu;
        public PlayerHUD playerHUD;

        public void LoadUI()
        {

        }

        public void LoadBaseCanvas(GameObject canvasPrefab) //Load canvas for all the menus
        {

        }

        public void LoadPlayerHUD(GameObject canvasPrefab) //Load canvas for all the in game HUD
        {
            playerHUD = Instantiate(canvasPrefab).GetComponent<PlayerHUD>();
        }

        public void LoadPauseMenu(GameObject prefab) //Load pause menu canvas 
        {
            pauseMenu = Instantiate(prefab).GetComponent<MENUScripts.PauseMenuUI>();
        }

        public void LoadWinMenu(GameObject prefab) //Load win menu canvas
        {
            winMenu = Instantiate(prefab).GetComponent<WinMenuUI>();
        }

        public void LoadLoseMenu(GameObject prefab) //Load lose menu canvas
        {
            loseMenu = Instantiate(prefab).GetComponent<LoseMenuUI>();
        }
    }
}