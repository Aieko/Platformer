using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager instance;
    [SerializeField] Text starText;
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private GameObject[] levelSelectionPanels;

    public int stars;
    
    public MapSelection[] mapSelections;
    public Text[] questStarsText;
    public Text[] lockedStarsText;
    public Text[] unlockedStarsText;

    
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
                stars++;
            }
        }

        UpdateNumOfStars();

        EventCenter.GetInstance().AddEventListener("RefreshStatistics", UpdateNumOfStars);
        EventCenter.GetInstance().AddEventListener("RefreshStatistics", UpdateLockedStarUI);
        EventCenter.GetInstance().AddEventListener("RefreshStatistics", UpdateUnlockedStarUI);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("RefreshStatistics", UpdateNumOfStars);
        EventCenter.GetInstance().RemoveEventListener("RefreshStatistics", UpdateLockedStarUI);
        EventCenter.GetInstance().RemoveEventListener("RefreshStatistics", UpdateUnlockedStarUI);
    }


    private void UpdateNumOfStars()
    {
        starText.text = stars.ToString();
    }

    private void UpdateLockedStarUI()
    {
        for (var i = 0; i < mapSelections.Length; i++)
        {
            questStarsText[i].text = mapSelections[i].questNumStars.ToString();

            if (!mapSelections[i].isUnlock)
            {
                lockedStarsText[i].text = stars.ToString() + "/" + (i+1) * 18;
            }
        }
    }

    private void UpdateUnlockedStarUI()
    {
        for (var i = 0; i < mapSelections.Length; i++)
        {
            unlockedStarsText[i].text = stars.ToString() + "/" + (i + 1) * 18;
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
            Debug.Log("You cannot open this map now. Please work hard to collect more stars");
        }
    }

    public void RefreshDataForUI()
    {
        EventCenter.GetInstance().EventTrigger("RefreshStatistics");
        EventCenter.GetInstance().EventTrigger("UpdateMap");
    }
}
