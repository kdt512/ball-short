using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages connections between event listeners and event invokers
/// </summary>
public static class EventManager
{
    #region Fields
    static Dictionary<NormalEventName, List<NormalEventInvoker>> normalInvokers
        = new Dictionary<NormalEventName, List<NormalEventInvoker>>();
    static Dictionary<NormalEventName, List<UnityAction>> normalListeners
        = new Dictionary<NormalEventName, List<UnityAction>>();

    static Dictionary<StringEventName, List<StringEventInvoker>> stringInvokers
        = new Dictionary<StringEventName, List<StringEventInvoker>>();
    static Dictionary<StringEventName, List<UnityAction<string>>> stringListeners
        = new Dictionary<StringEventName, List<UnityAction<string>>>();

    static bool isInitial = false;
    #endregion

    public static bool IsInitial { get { return isInitial; } }

    public static void Initialize()
    {
        Debug.Log("EventManager Initialize");
        if (isInitial)
        {
            Debug.LogWarning("EventManager had initialled");
            return;
        }
        isInitial = true;
        // create empty lists for all the dictionary entries
        foreach (NormalEventName name in Enum.GetValues(typeof(NormalEventName)))
        {
            if (!normalInvokers.ContainsKey(name))
            {
                normalInvokers.Add(name, new List<NormalEventInvoker>());
                normalListeners.Add(name, new List<UnityAction>());
            }
            else
            {
                normalInvokers[name].Clear();
                normalListeners[name].Clear();
            }
        }

        foreach (StringEventName name in Enum.GetValues(typeof(StringEventName)))
        {
            if (!stringInvokers.ContainsKey(name))
            {
                stringInvokers.Add(name, new List<StringEventInvoker>());
                stringListeners.Add(name, new List<UnityAction<string>>());
            }
            else
            {
                stringInvokers[name].Clear();
                stringListeners[name].Clear();
            }
        }
    }

    public static void AddInvoker(NormalEventName eventName, NormalEventInvoker invoker)
    {
        // add listeners to new invoker and add new invoker to dictionary
        foreach (UnityAction listener in normalListeners[eventName])
        {
            invoker.AddListener(listener);
        }
        normalInvokers[eventName].Add(invoker);
    }

    public static void AddListener(NormalEventName eventName, UnityAction listener)
    {
        // add as listener to all invokers and add new listener to dictionary
        foreach (NormalEventInvoker invoker in normalInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
        normalListeners[eventName].Add(listener);
    }

    /// <summary>
    /// Removes the given invoker for the given event name
    /// </summary>
    /// <param name="eventName">event name</param>
    /// <param name="invoker">invoker</param>
    public static void RemoveInvoker(NormalEventName eventName, NormalEventInvoker invoker)
    {
        // remove invoker from dictionary
        normalInvokers[eventName].Remove(invoker);
    }

    public static void AddInvoker(StringEventName eventName, StringEventInvoker invoker)
    {
        Debug.Log("AddInvoker " + eventName);
        // add listeners to new invoker and add new invoker to dictionary
        foreach (UnityAction<string> listener in stringListeners[eventName])
        {
            invoker.AddListener(eventName, listener);
        }
        stringInvokers[eventName].Add(invoker);
    }

    public static void AddListener(StringEventName eventName, UnityAction<string> listener)
    {
        // add as listener to all invokers and add new listener to dictionary
        foreach (StringEventInvoker invoker in stringInvokers[eventName])
        {
            invoker.AddListener(eventName, listener);
        }
        stringListeners[eventName].Add(listener);
    }

    /// <summary>
    /// Removes the given invoker for the given event name
    /// </summary>
    /// <param name="eventName">event name</param>
    /// <param name="invoker">invoker</param>
    public static void RemoveInvoker(StringEventName eventName, StringEventInvoker invoker)
    {
        // remove invoker from dictionary
        if (stringInvokers.ContainsKey(eventName))
            stringInvokers[eventName].Remove(invoker);
    }
}