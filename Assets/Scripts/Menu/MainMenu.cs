﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        for (int i = 1; i < 72; i++)
        {
            PlayerPrefs.SetInt("Seed" + i, 0);
        }

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

       
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
