using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : EnemyBase
{
    public float speed;
    public float sightRange;
    private bool inSightRange;
    private bool bounce;
    void Update()
    {
        inSightRange = distanceFromPlayer < sightRange;
        if (!inSightRange) Idle();
        if (inSightRange) Chase();
        if (inSightRange && bounce) Bounce();
    }
    void Idle()
    {

    }
    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    void Bounce()
    {
        
    }
}
