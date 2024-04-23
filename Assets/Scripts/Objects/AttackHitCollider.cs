using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitCollider : MonoBehaviour
{
    public bool active = true;
    [SerializeField] protected int damage = 20;
    [SerializeField] protected Health.DamageType type;
    [SerializeField] protected bool deactivateOnHit = true;
    [SerializeField] protected bool deactivateOnHitWithHealth;
    [SerializeField] protected bool useCollider = true;
    [SerializeField] protected bool useTrigger = true;
    //[SerializeField] Health.EntityType willHitTypes;

    protected void OnTriggerEnter(Collider other) { if (useTrigger) Hit(other.gameObject); }

    protected void OnCollisionEnter(Collision collision) { if (useCollider) Hit(collision.gameObject); }

    protected virtual void Hit(GameObject subject)
    {
        if(!active) return;

        Health health = subject.GetComponent<Health>();
        if (health)
        {
            health.Damage(damage, type, this);
        }

        if (deactivateOnHit)
        {
            if((deactivateOnHitWithHealth && health) || !deactivateOnHitWithHealth)gameObject.SetActive(false);
        }
    }
}
