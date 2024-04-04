using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : EnemyBase
{
    public float speed;
    public float sightRange;
    private bool inSightRange;
    [SerializeField] string playerTag;
    [SerializeField] float bounceForce;
    void Update()
    {
        inSightRange = distanceFromPlayer < sightRange;
        if (!inSightRange) Idle();
        if (inSightRange) Chase();
    }
    void Idle()
    {

    }
    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == playerTag)
        {
            Rigidbody otherRB = collision.rigidbody;
            otherRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
        }
    }
}
