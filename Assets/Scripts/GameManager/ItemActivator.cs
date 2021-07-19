﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemActivator : MonoBehaviour
{
    
    // --------------------------------------------------
    // Variables:

    [SerializeField]
    private int distanceFromPlayer;

    private GameObject player;

    private List<ActivatorItem> activatorItems;

    public List<ActivatorItem> addList;

    // --------------------------------------------------

    void Start()
    {
        player = GameObject.Find("Player");
        activatorItems = new List<ActivatorItem>();
        addList = new List<ActivatorItem>();

        AddToList();
    }

    void AddToList()
    {
        if (addList.Count > 0)
        {
            foreach (ActivatorItem item in addList)
            {
                if (item.item != null)
                {
                    activatorItems.Add(item);
                }
            }

            addList.Clear();
        }

        StartCoroutine("CheckActivation");
    }

    IEnumerator CheckActivation()
    {
        List<ActivatorItem> removeList = new List<ActivatorItem>();

        if (activatorItems.Count > 0)
        {
            foreach (ActivatorItem item in activatorItems)
            {
                if(item.item.CompareTag("Enemy"))
                {
                   Entity enemy = item.item.GetComponent<Entity>();
                
                    if (Vector3.Distance(player.transform.position, item.item.transform.position) > distanceFromPlayer)
                    {
                       
                        if (item.item == null)
                        {
                            removeList.Add(item);
                        }
                        else
                        {
                            if(enemy.anim.GetBool("Idle"))
                            enemy.enabled = false;
                        }
                    }
                    else
                    {
                        
                        if (item.item == null)
                        {
                            removeList.Add(item);
                        }
                        else
                        {
                          enemy.enabled = true;
                        }
                    }
                }
                else
                if (Vector3.Distance(player.transform.position, item.item.transform.position) > distanceFromPlayer)
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.item.SetActive(false);
                    }
                }
                else
                {
                    if (item.item == null && item.item.CompareTag("Enemy"))
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        item.item.SetActive(true);
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

        yield return new WaitForSeconds(0.01f);

        if (removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                activatorItems.Remove(item);
            }
        }

        yield return new WaitForSeconds(0.01f);

        AddToList();
    }
}

public class ActivatorItem
{
    public GameObject item;
}
