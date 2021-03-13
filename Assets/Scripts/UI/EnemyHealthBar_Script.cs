using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar_Script : MonoBehaviour
{
    public Slider slider;
    public GameObject HealthBar;
   
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void EnableHealthBar(bool Status)
    {
        HealthBar.SetActive(Status);
    }
}
