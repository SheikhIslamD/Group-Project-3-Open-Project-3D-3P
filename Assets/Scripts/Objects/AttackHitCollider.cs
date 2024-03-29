using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitCollider : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] Health.DamageType type;
    [SerializeField] bool deactivateOnHit = true;
    [SerializeField] bool deactivateOnHitWithHealth;
    [SerializeField] bool useCollider = true;
    [SerializeField] bool useTrigger = true;
    //[SerializeField] Health.EntityType willHitTypes;

    private void OnTriggerEnter(Collider other) { if (useTrigger) Hit(other.gameObject); }

    private void OnCollisionEnter(Collision collision) { if (useCollider) Hit(collision.gameObject); }

    void Hit(GameObject subject)
    {
        Health health = subject.GetComponent<Health>();
        if (health) health.Damage(damage, type);

        if (deactivateOnHit)
        {
            if((deactivateOnHitWithHealth && health) || !deactivateOnHitWithHealth) gameObject.SetActive(false);
        }
    }
}
