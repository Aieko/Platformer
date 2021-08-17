﻿using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();

        if(core == null)
        {
            Debug.LogError("Where's no Core on the parent");
        }
    }
}