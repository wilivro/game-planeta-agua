using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    public class MyEvent : UnityEvent<object[]> {};

    private Dictionary <string, MyEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init (); 
                }
            }

            return eventManager;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, MyEvent>();
        }
    }

    public static void AddListener (string eventName, UnityAction<object[]> listener)
    {
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener (listener);
        } 
        else
        {
            thisEvent = new MyEvent ();
            thisEvent.AddListener (listener);
            instance.eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void RemoveListener (string eventName, UnityAction<object[]> listener)
    {
        if (eventManager == null) return;
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void Trigger (string eventName, object[] param = null)
    {
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Invoke (param);
        }
    }
}