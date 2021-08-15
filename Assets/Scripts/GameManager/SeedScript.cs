using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
  
 
    private void OnTriggerEnter2D(Component collision)
    {
        if (!collision.CompareTag("Player")) return;

        GameManager.instance.PickUpSeed(transform.name);

        Destroy(gameObject);
    }
}
