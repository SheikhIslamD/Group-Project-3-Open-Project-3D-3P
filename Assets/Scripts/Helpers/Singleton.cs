using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Class is a Base Template for the Singleton Design pattern.
//Any Class that inherets from this will be a Singleton.
//Singletons make it impossible for a script to exist twice.
//Useful for managers that multiples of can cause weirdness.

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _Instance;
    public static T instance { get { 
            if (_Instance == null)
            {
                T findAttempt = FindFirstObjectByType<T>();
                if (findAttempt != null)
                {
                    findAttempt.Awake();
                    return findAttempt;
                } 
                else
                {
                    Debug.LogWarning("There's no Singleton of that type in this scene.");
                    return null;
                }
            }
            else return _Instance; } }
    public static T Get() => instance;
    public static T i => instance;
    public static T single => instance;

    public static bool IsInitialized
    {
        get { return _Instance != null; }
    }

    /// <summary>
    /// This is the Unity Function which runs some code necessary for Singleton Function. Use OnAwake() instead.
    /// </summary>
    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogError(
                "Something or someone is attempting to create a second " + 
                typeof(T).ToString() +
                ". Which is a Singleton. If you wish to reset the " +
                typeof(T).ToString() +
                ", destroy the first before instantiating its replacement. The duplicate " +
                typeof(T).ToString() +
                " will now be Deleted."
                );

            Object.Destroy(this);
        }
        else
        {
            _Instance = (T) this;
            OnAwake();
            //Debug.Log(
            //    "The " +
            //    typeof(T).ToString() +
            //    " Singleton has been successfully Created/Reset."
            //    );
        }
    }

    protected virtual void OnAwake(){}

    /// <summary>
    /// This is the Unity Function which runs some code necessary for Singleton Function. Use OnDestroyed() instead.
    /// </summary>
    private void OnDestroy()
    {
        if (_Instance == this)
        {
            _Instance = null;
        }
        OnDestroyed();
    }
    protected virtual void OnDestroyed() {}

    /// <summary>
    /// Destroys the instance of this singleton, wherever it is.
    /// </summary>
    /// <param name="leaveGameObject"> Whether the Game Object that contains the Singleton is left behind.</param>
    public static void Destroy(bool leaveGameObject = false)
    {
        if (instance == null) return;
        if(!leaveGameObject)
        {
            MonoBehaviour.Destroy(instance.gameObject);
        }
        else
        {
            Object.Destroy(instance);
            instance.OnDestroy();
        }
    }

    /// <summary>
    /// Very Dangerous. Do not use if you don't know what you're doing.
    /// </summary>
    public void Reset()
    {
        GameObject obj = _Instance.gameObject;
        Destroy(true);
        Create(obj);
    }

    /// <summary>
    /// Creates an instance of this singleton and attaches it to the desired Game Object.
    /// </summary>
    /// <param name="object"> The object you are attaching the singleton to.</param>
    /// <param name="replace"> Whether or not this will forcibly replace an existing instance with the new one.</param>
    public static void Create(GameObject @object, bool replace = false)
    {
        if (!replace) if (instance != null) return;
            else Destroy(true);
        @object.AddComponent<T>().Awake();
    }

}
