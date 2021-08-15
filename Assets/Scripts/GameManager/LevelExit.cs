using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Component collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Finished", 1);

            foreach (var seed in GameManager.instance.pickedSeeds)
            {
                PlayerPrefs.SetInt(seed, 1);
            }

            if(SceneManager.GetActiveScene().buildIndex == 6 || SceneManager.GetActiveScene().buildIndex == 12 
                                                              || SceneManager.GetActiveScene().buildIndex == 18 || SceneManager.GetActiveScene().buildIndex == 24)
            {
                Debug.Log("It was finale level on this map. Go to Main Menu and select next Map!");
                return;
            }

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("Scene doesn't exists.");
        }
    }
}
