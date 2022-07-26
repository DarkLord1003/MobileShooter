using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class EventManager : MonoBehaviour
{
    //EventManager reference
    private static EventManager _instance;

    //Events
    private Dictionary<string, UnityEvent> _events;

    public static EventManager Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        GetStartReferences();
        Init();
    }

    public static void StartListening(string eventName,UnityAction listen)
    {
        UnityEvent unityEvent;

        if(Instance._events.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.AddListener(listen);
        }
        else
        {
            unityEvent = new UnityEvent();
            unityEvent.AddListener(listen);
            Instance._events.Add(eventName,unityEvent);
        }
    }

    public static void StopListening(string eventName,UnityAction listen)
    {
        if (Instance == null)
            return;

        UnityEvent unityEvent;

        if(Instance._events.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.RemoveListener(listen);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent unityEvent;

        if(Instance._events.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.Invoke();
        }
    }


    #region - GetReference -

    private void GetStartReferences()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region - Init -

    private void Init()
    {
        _events = new Dictionary<string, UnityEvent>();
    }

    #endregion
}
