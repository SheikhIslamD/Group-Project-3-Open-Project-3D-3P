using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : EnemyBase
{
    public float speed;
    public float sightRange;
    private bool inSightRange;

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
        transform.position = Vector3.MoveTowards(transform.position, player.centerPos, speed * Time.deltaTime);
    }
}
