using System.Collections;
using UnityEngine;

public class SwordHitCollider : AttackHitCollider
{
    [SerializeField] AudioCaller audioC;

    public void Awake()
    {
        type = Health.DamageType.Melee;
        deactivateOnHit = false;
        deactivateOnHitWithHealth = false;
        useCollider = false;
        useTrigger = true;
    }


    protected new void OnCollisionEnter(Collision other) { Hit(other.gameObject); }
    protected new void OnTriggerEnter(Collider other) { Hit(other.gameObject); }

    protected override void Hit(GameObject subject)
    {
        base.Hit(subject);

        if (ReflectableProjectile.Reflect(subject)) audioC.PlaySound("Parry");

    }
}
