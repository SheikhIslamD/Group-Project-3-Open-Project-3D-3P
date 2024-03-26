using System.Collections;
using UnityEngine;


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
        if (isReflected)
        {
            MakeNormal();
        }
    }

}
