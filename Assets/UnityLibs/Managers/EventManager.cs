using System;
using System.Collections.Generic;
using UnityEngine;
public class EventManager : MonoBehaviour
{
    private void Awake()
    {
        subscribers = new Dictionary<string, List<object>>();
    }

    private static IDictionary<string, List<object>> subscribers = new Dictionary<string, List<object>>();

    public static void Subscribe<T, P>(string message, Action<T, P> callback)
    {
        if (subscribers.ContainsKey(message))
        {
            subscribers[message].Add(callback);
        }
        else
        {
            subscribers[message] = new List<object>();
            subscribers[message].Add(callback);
        }
    }
    public static void Subscribe<T>(string message, Action<T> callback)
    {
        if (subscribers.ContainsKey(message))
        {
            subscribers[message].Add(callback);
        }
        else
        {
            subscribers[message] = new List<object>();
            subscribers[message].Add(callback);
        }
    }

    public static void Subscribe(string message, Action callback)
    {
        if (subscribers.ContainsKey(message))
        {
            subscribers[message].Add(callback);
        }
        else
        {
            subscribers[message] = new List<object>();
            subscribers[message].Add(callback);
        }
    }

    public static void Invoke<T>(string message, T param)
    {

        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {
                try
                {
                    Action<T> callback = (Action<T>)callbacks[i];

                    if(callback == null)
                    {
                        Debug.LogError("ERROR ICI");
                    }
                    else
                    {
                        callback(param);
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError("CALL BACK FAILED AT WITH MESSAGE " + message + ", " + callbacks[i].ToString());
                    Debug.LogError(e);
                }

            }
        }
    }

    public static void Invoke<T, P>(string message, T param1, P param2)
    {

        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {
                try
                {
                    Action<T, P> callback = (Action<T, P>)callbacks[i];
                   

                    if (callback == null)
                    {
                        Debug.LogError("ERROR ICI");
                    }
                    else
                    {
                        callback(param1, param2);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("CALL BACK FAILED AT WITH MESSAGE " + message + ", " + callbacks[i].ToString());
                    Debug.LogError(e);
                }
            }
        }
    }

    public static void Invoke(string message)
    {

        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {
                try
                {
                    Action callback = (Action)callbacks[i];

                    if (callback == null)
                    {
                        Debug.LogError("ERROR ICI");
                    }
                    else
                    {
                        callback();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("CALL BACK FAILED AT WITH MESSAGE " + message + ", " + callbacks[i].ToString());
                    Debug.LogError(e);
                }
            }
        }
    }

    public static void Unsubscribe<T, P>(string message, Action<T, P> callback)
    {
        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {

                Action<T, P> tmpCallback = (Action<T, P>)callbacks[i];

                if (tmpCallback == callback)
                {

                    callbacks.RemoveAt(i);

                    break;
                }
            }
        }
    }

    public static void Unsubscribe<T>(string message, Action<T> callback)
    {
        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {

                Action<T> tmpCallback = (Action<T>)callbacks[i];

                if (tmpCallback == callback)
                {

                    callbacks.RemoveAt(i);

                    break;
                }
            }
        }
    }

    public static void Unsubscribe(string message, Action callback)
    {
        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {
                Action tmpCallback = (Action)callbacks[i];

                if (tmpCallback == callback)
                {
                    callbacks.RemoveAt(i);
                    break;
                }
            }
        }
    }
}