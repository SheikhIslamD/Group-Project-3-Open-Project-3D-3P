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
    /// This method is used for Setup of the Pooled Object Instance after it is Activated. In the default base of this script this method does nothing, if not overridden Setup is the responsibility of the script calling Pump();
    /// </summary>
    public virtual void Prepare() { }

    public virtual void Prepare_Basic(Vector3 position, Vector3 direction, Vector3 velocity)
    {
        transform.position = position;
        transform.eulerAngles = direction;

        Rigidbody rigid = rb;
        if (rigid == null) return;

        rigid.velocity = transform.TransformDirection(velocity);
        rigid.angularVelocity = Vector3.zero;

    }

    /// <summary>
    /// An accesible function for another script to disable this Poolable Object without necessarily Deactivating the Game Object.
    /// </summary>
    public void Disable(bool deactivateGameObject = false)
    {
        Active = false;
        onDeactivate(this);
        if(deactivateGameObject) gameObject.SetActive(false);

        if(pool == null) Destroy(gameObject);
    }
    void OnDisable() => Disable();

    public Rigidbody rb => GetComponent<Rigidbody>();



}