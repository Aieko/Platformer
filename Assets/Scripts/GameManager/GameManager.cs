using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[SerializeField] private Transform respawnPoint;
    //[SerializeField] private float respawnTime;
    //private float respawnTimeStart;
    //private bool respawn;


    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] seed;

    public List<string> pickedSeeds { get; private set; }

    private GameObject gameMenu;

    private CinemachineVirtualCamera CVC;

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

        for (int i = 0; i <= 2; i++)
        {
            if (PlayerPrefs.GetInt(seed[i].name) == 1)
            {
                Destroy(seed[i]);
            }
        }

        gameMenu = GameObject.FindGameObjectWithTag("GameMenu");
    }

    private void Start()
    {
        CVC = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();

        Time.timeScale = 1f;

        pickedSeeds = new List<string>();

    }

    public void OnDeath()
    {
        gameMenu.GetComponent<GameMenuManager>().continueButton.SetActive(false);
        gameMenu.GetComponent<Canvas>().enabled = true;
    }

    public void PickUpSeed(string seed)
    {
        pickedSeeds.Add(seed);
    }

    #region Unused

    /*
 public void Respawn()
 {
     respawnTimeStart = Time.time;
     respawn = true;
 }

 private void CheckRespawn()
 {
     if (!(Time.time >= respawnTimeStart + respawnTime) || !respawn) return;
     var playerTemp = Instantiate(player, respawnPoint);

     playerTemp.transform.SetParent(playerTemp.transform);
     CVC.m_Follow = playerTemp.transform;
     respawn = false;
 }*/

    #endregion
}