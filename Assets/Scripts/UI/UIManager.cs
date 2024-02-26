using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PauseMenuUI pauseMenu;
    public WinMenuUI winMenu;
    public LoseMenuUI loseMenu;
    public PlayerHUD playerHUD;

    public void LoadUI()
    {
        
    }
    
    public void LoadBaseCanvas(GameObject canvas) //Load canvas for all the menus
    {
        
    }

    public void LoadPlayerHUD(GameObject canvas) //Load canvas for all the in game HUD
    {
        playerHUD = canvas.GetComponent<PlayerHUD>();
    }
    
    public void LoadPauseMenu(GameObject prefab) //Load pause menu canvas 
    {
        pauseMenu = prefab.GetComponent<PauseMenuUI>();
    }

    public void LoadWinMenu(GameObject canvas) //Load win menu canvas
    {
        
    }

    public void LoadLoseMenu(GameObject canvas) //Load lose menu canvas
    {
        
    }
}
