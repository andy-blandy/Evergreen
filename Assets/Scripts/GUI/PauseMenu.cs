using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;

    public GameObject pauseCanvas;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject volumeMenu;

    void OnEnable()
    {
        PlayerInput.OnBack += PauseButton;
    }

    void OnDisable()
    {
        PlayerInput.OnBack -= PauseButton;
    }

    public void PauseButton()
    {
        if (Player.instance.inShop)
        {
            return;
        }

        if (isPaused)
        {
            UnpauseGame();
        } else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        isPaused = true;

        // Freeze player
        Player.instance.FreezePlayer(true);

        // Show menu
        pauseCanvas.SetActive(true);
        OpenMenu(mainMenu);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;

        // Unfreeze player
        Player.instance.FreezePlayer(false);

        // Hide menu
        pauseCanvas.SetActive(false);
    }

    public void CloseAllMenus()
    {
        foreach (Transform child in pauseCanvas.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OpenMenu(GameObject menuToOpen)
    {
        CloseAllMenus();
        menuToOpen.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
