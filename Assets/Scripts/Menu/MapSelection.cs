using UnityEngine;

public class MapSelection : MonoBehaviour
{
    public bool isUnlock { get; private set; }

    [SerializeField] private GameObject lockGO;
    [SerializeField] private GameObject unlockGO;

    public int questNumStars;
    [SerializeField] private int startLevel;
    [SerializeField] private int endLevel;


    private void Awake()
    {
        isUnlock = false;
        EventCenter.GetInstance().AddEventListener("UpdateMap", UpdateLockOnMap);
        EventCenter.GetInstance().AddEventListener("UpdateMap", UpdateMapStatus);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener("UpdateMap", UpdateLockOnMap);
        EventCenter.GetInstance().RemoveEventListener("UpdateMap", UpdateMapStatus);
    }

    private void UpdateMapStatus()
    {
        if (isUnlock)
        {
            unlockGO.SetActive(true);
            lockGO.SetActive(false);
        }
        else
        {
            unlockGO.SetActive(false);
            lockGO.SetActive(true);
        }
    }

    private void UpdateLockOnMap()
    {
        isUnlock = MenuUIManager.instance.stars >= questNumStars;
    }
}
