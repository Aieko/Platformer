using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        for (int i = 1; i < 72; i++)
        {
            PlayerPrefs.SetInt("Seed" + i, 0);
        }

        for (int i = 1; i <= 12; i++)
        {
            PlayerPrefs.SetInt("Level" + i + "Finished", 0);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
