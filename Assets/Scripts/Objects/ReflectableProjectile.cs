using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PoolableObject))]
public class ReflectableProjectile : MonoBehaviour
{
    public Transform sender => GetComponent<PoolableObject>().pool.transform;
    [SerializeField] LayerMask normalMask;
    [SerializeField] LayerMask reflectedMask;
    bool isReflected;

    public void MakeReflected()
    {
        GetComponent<Collider>().excludeLayers = reflectedMask;
        isReflected = true;
    }

    public void MakeNormal()
    {
        GetComponent<Collider>().excludeLayers = normalMask;
        isReflected = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isReflected && collision.collider.GetComponent<Health>() != null)
        {
            MakeNormal();
        }
    }

}
