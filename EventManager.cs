using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    // Data Members
    private Dictionary<string, UnityEvent> eventDict;
    private static EventManager eventManager;

    // init
    void Init ()
    {
        if (eventDict == null)
        {
            eventDict = new Dictionary<string, UnityEvent>();
        }
    }

    // Getter Method
        // check if eventManager exists
        // if not find script in scene and initialize

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                // no EventManager script attached to object in scene
                if (!eventManager)
                {
                    Debug.LogError("Attach EventManager to an object in game scene");
                } else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    // Start Listning
    public static void StartListening (string eventName, UnityAction listner)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDict.TryGetValue (eventName, out thisEvent))
        {   
            // event already in dictionary
            thisEvent.AddListener(listner);
        } else
        {
            // new event - add to dict
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listner);
            instance.eventDict.Add(eventName, thisEvent);
        }
    }

    // Stop Listening
    public static void StopListening(string eventName, UnityAction listner)
    {   
        // if there is no manager exit
        if (eventManager == null) return;

        UnityEvent thisEvent = null;

        if (instance.eventDict.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listner);
        }

    }

    // Trigger Event
    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDict.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

}
