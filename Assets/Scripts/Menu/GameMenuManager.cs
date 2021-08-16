using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    private GameObject gameMenu;

    private GameObject player;

    public GameObject continueButton;

    private void Awake()
    {
        gameMenu = GameObject.FindGameObjectWithTag("GameMenu");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (!continueButton.activeInHierarchy)
        {
            continueButton.SetActive(true);
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");

    }

    public void Continue()
    {
        Time.timeScale = 1f;

        if (player)
        {
            player.GetComponent<PlayerInputHandler>().ActivateInput(true);
            player.GetComponent<PlayerInputHandler>().isGamePaused = false;
        }

        gameMenu.GetComponent<Canvas>().enabled = false;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
