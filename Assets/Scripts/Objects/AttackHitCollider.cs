using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitCollider : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] Health.DamageType type;
    [SerializeField] bool deactivateOnHit;
    //[SerializeField] Health.EntityType willHitTypes;
    
    private void OnTriggerEnter(Collider other) => Hit(other.gameObject);

    private void OnCollisionEnter(Collision collision) => Hit(collision.gameObject);

    void Hit(GameObject subject)
    {
        Health health = subject.GetComponent<Health>();
        if (health) health.Damage(damage, type);

        if (deactivateOnHit) gameObject.SetActive(false);
        Debug.Log("Hit " + subject);
    }
}
