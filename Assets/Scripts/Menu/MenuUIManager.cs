using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager instance;
    [SerializeField] Text seedText;
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private GameObject[] levelSelectionPanels;

    public int seeds;
    
    public MapSelection[] mapSelections;
    public Text[] questSeedsText;
    public Text[] lockedSeedsText;
    public Text[] unlockedSeedsText;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        for (int i = 1; i <= 72; i++)
        {
            if (PlayerPrefs.GetInt("Seed" + i ) == 1)
            {
                seeds++;
            }
        }

        UpdateNumOfSeeds();

        EventCenter.GetInstance().AddEventListener("RefreshStatistics", UpdateNumOfSeeds);
        EventCenter.GetInstance().AddEventListener("RefreshStatistics", UpdateLockedSeedUI);
        EventCenter.GetInstance().AddEventListener("RefreshStatistics", UpdateUnlockedSeedUI);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("RefreshStatistics", UpdateNumOfSeeds);
        EventCenter.GetInstance().RemoveEventListener("RefreshStatistics", UpdateLockedSeedUI);
        EventCenter.GetInstance().RemoveEventListener("RefreshStatistics", UpdateUnlockedSeedUI);
    }


    private void UpdateNumOfSeeds()
    {
        seedText.text = seeds.ToString();
    }

    private void UpdateLockedSeedUI()
    {
        for (var i = 0; i < mapSelections.Length; i++)
        {
            questSeedsText[i].text = mapSelections[i].questNumStars.ToString();

            if (!mapSelections[i].isUnlock)
            {
                lockedSeedsText[i].text = seeds.ToString() + "/" + (i+1) * 18;
            }
        }
    }

    private void UpdateUnlockedSeedUI()
    {
        for (var i = 0; i < mapSelections.Length; i++)
        {
            unlockedSeedsText[i].text = seeds.ToString() + "/" + (i + 1) * 18;
        }
    }

    public void SelectLevel(int levelNum)
    {
        if (PlayerPrefs.GetInt("Level" + (levelNum - 1) + "Finished") == 1
            || levelNum == 1 || levelNum == 7 || levelNum == 13 || levelNum == 19)
        {
            SceneManager.LoadScene("Level" + levelNum);
        }
        else
        {
            Debug.Log("You cannot open this level now. Please work hard to end up previous levels");
        }
    }

    public void PressMapButton(int mapIndex)
    {
        if (mapSelections[mapIndex].isUnlock == true)
        {
            levelSelectionPanels[mapIndex].gameObject.SetActive(true);
            mapSelectionPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("You cannot open this map now. Please work hard to collect more seeds");
        }
    }

    public void RefreshDataForUI()
    {
        EventCenter.GetInstance().EventTrigger("RefreshStatistics");
        EventCenter.GetInstance().EventTrigger("UpdateMap");
    }
}
