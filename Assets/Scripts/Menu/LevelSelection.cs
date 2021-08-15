using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public GameObject lockGO;

    public GameObject[] emptyStars;

    public GameObject[] filledStars;

    public bool isUnlock { get; private set; }
    private int levelNumber;
    private int[] starsNum;

    private void Awake()
    {
        isUnlock = false;
        levelNumber = int.Parse(gameObject.name);

        if (levelNumber == 1 || levelNumber == 7 || levelNumber == 13 || levelNumber == 19)
        {
            isUnlock = true;
            lockGO.SetActive(false);
        }

        var previousLevelNumber = levelNumber - 1;

        if (PlayerPrefs.GetInt("Level" + previousLevelNumber + "Finished") == 1)
        {
            lockGO.SetActive(false);
            isUnlock = true;
        }

        FillTheStar(levelNumber);
    }

    private void FillTheStar(int levelNum)
    {
        int workspace = levelNum * 3;

        starsNum = new int[] {workspace, workspace - 1, workspace - 2};

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetInt("Seed" + starsNum[i]) == 1)
            {
                emptyStars[i].SetActive(false);
                filledStars[i].SetActive(true);
            }
            else
            {
                filledStars[i].SetActive(false);
                if(isUnlock) { emptyStars[i].SetActive(true); }
                
            }
        }
    }
}
