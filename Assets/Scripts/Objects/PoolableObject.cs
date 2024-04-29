using System;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{

    [HideInInspector] public ObjectPool pool;
    [HideInInspector] public bool Active;
    [HideInInspector] public float timeExisting;
    [SerializeField] bool makeSoundOnContact;

    /// <summary>
    /// If nothing calls this action when this object instance is done the object will never be available for reuse.
    /// </summary>
    public Action<PoolableObject> onDeactivate;

    /// <summary>
    /// This method is used for Setup of the Pooled Object Instance after it is Activated. In the default base of this script this method does nothing, if not overridden Setup is the responsibility of the script calling Pump();
    /// </summary>
    public virtual void Prepare() { }

    public virtual void Prepare_Basic(Vector3 position, Vector3 direction, Vector3 velocity, bool relative = true)
    {
        transform.position = position;
        transform.eulerAngles = direction;

        Rigidbody rigid = rb;
        if (!rigid) return;

        rigid.velocity = relative ? transform.TransformDirection(velocity) : velocity;
        rigid.angularVelocity = Vector3.zero;

    }

    /// <summary>
    /// An accesible function for another script to disable this Poolable Object without necessarily Deactivating the Game Object.
    /// </summary>
    public void Disable(bool deactivateGameObject = false)
    {
        if (!gameObject.scene.isLoaded) return;
        bool wasActive = Active;
        Active = false;
        if (onDeactivate.GetInvocationList().Length > 0 && wasActive) onDeactivate(this);
        if (deactivateGameObject) gameObject.SetActive(false);

        if (makeSoundOnContact) GetComponent<AudioCaller>().PlaySound("Impact");
        if (!pool) Destroy(gameObject);
    }

    private void OnDisable() { if (Active) Disable(); }
    public Rigidbody rb => GetComponent<Rigidbody>();

    public static PoolableObject IsPooled(GameObject subject)
    {
        PoolableObject pool = subject.GetComponent<PoolableObject>(); 
        if (!pool) return null;
        if (!pool.pool) return null;
        return pool;
    }

}