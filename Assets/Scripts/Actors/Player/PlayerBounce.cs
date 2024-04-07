using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    [SerializeField] string enemyTag;
    [SerializeField] float bounceForce;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == enemyTag)
        {
            Rigidbody otherRB = collision.rigidbody;
            otherRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
        }
    }
}
