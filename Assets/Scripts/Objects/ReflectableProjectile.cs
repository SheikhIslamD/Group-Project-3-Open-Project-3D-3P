﻿using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(PoolableObject))]
public class ReflectableProjectile : MonoBehaviour
{
    public Transform sender => GetComponent<PoolableObject>().pool.transform;
    [SerializeField] LayerMask normalMask;
    [SerializeField] LayerMask reflectedMask;
    bool isReflected;

    public static bool Reflect(GameObject target)
    {
        ReflectableProjectile reflect = target.GetComponent<ReflectableProjectile>();
        if (reflect == null) return false;

        reflect.MakeReflected();
        Rigidbody rb = reflect.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Vector3 direction = reflect.sender.position - rb.position;
        rb.AddForce(direction.normalized * 1400);
        return true;
    }

    public void MakeReflected()
    {
        Debug.Log("refele");
        var col = GetComponent<Collider>();
        col.includeLayers = reflectedMask;
        col.excludeLayers = ~reflectedMask;
        isReflected = true;
    }

    public void MakeNormal()
    {
        var col = GetComponent<Collider>();
        col.includeLayers = normalMask;
        col.excludeLayers = ~normalMask;
        isReflected = false;
    }

    private void OnCollisionEnter(Collision collision) => Collide(collision.gameObject);
    private void OnTriggerEnter(Collider other) => Collide(other.gameObject);
    void Collide(GameObject obj)
    {
        if (isReflected && obj.GetComponent<Health>() != null)
        {
            MakeNormal();
        }
    }
}
