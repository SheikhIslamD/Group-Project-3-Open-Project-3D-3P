using System;
using UnityEngine;
using UnityEngine.Pool;

public class PoolableObject : MonoBehaviour{

    [HideInInspector] public ObjectPool pool;
    [HideInInspector] public bool Active;
    [HideInInspector] public float timeExisting;

    /// <summary>
    /// If nothing calls this action when this object instance is done the object will never be available for reuse.
    /// </summary>
    public Action<PoolableObject> onDeactivate;

    /// <summary>
    /// An accesible function for another script to disable this Poolable Object without necessarily Deactivating the Game Object.
    /// </summary>
    public void Disable(bool deactivateGameObject = false)
    {
        Active = false;
        onDeactivate(this);
        if(deactivateGameObject) gameObject.SetActive(false);

    }
    void OnDisable() => Disable();
    void OnDeActivate() => Disable();
}