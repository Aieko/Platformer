using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public GameObject lockGO;

    public GameObject[] emptySeeds;

    public GameObject[] filledSeeds;

    public bool isUnlock { get; private set; }
    private int levelNumber;
    private int[] seedsNum;

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

        seedsNum = new int[] {workspace - 2, workspace - 1, workspace};

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetInt("Seed" + seedsNum[i]) == 1)
            {
                emptySeeds[i].SetActive(false);
                filledSeeds[i].SetActive(true);
            }
            else
            {
                filledSeeds[i].SetActive(false);
                if(isUnlock) { emptySeeds[i].SetActive(true); }
                
            }
        }
    }
}
