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


    protected new void OnTriggerEnter(Collider other) { Hit(other.gameObject); }

    protected override void Hit(GameObject subject)
    {
        base.Hit(subject);
        Debug.Log("Test");

        ReflectableProjectile reflect = subject.GetComponent<ReflectableProjectile>();
        if (reflect != null)
        {
            audioC.PlaySound("Parry");

            Rigidbody rb = subject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            Vector3 direction = reflect.sender.position - rb.position;
            rb.AddForce(direction.normalized * 1400);
            reflect.MakeReflected();
        }

    }
}
