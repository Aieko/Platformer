using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter
{
    private static EventCenter instance;
    private Dictionary<string, UnityAction> eventDict = new Dictionary<string, UnityAction>();

    public static EventCenter GetInstance()
    {
        return instance ?? (instance = new EventCenter());
    }

    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDict.ContainsKey(name))
        {
            eventDict[name] += action;
        }
        else
        {
            eventDict.Add(name, action);
        }
    }

    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDict.ContainsKey(name))
        {
            eventDict[name] -= action;
        }
    }

    public void EventTrigger(string name)
    {
        if (eventDict.ContainsKey(name))
            eventDict[name]();
    }



    public void Clear()
    {
        eventDict.Clear();
    }

}