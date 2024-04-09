using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3Helper;

public class EnemyBounce : EnemyBase
{
    //Config
    [SerializeField] float speed;
    [SerializeField] float bounceSpeed;
    [SerializeField] float dampeningRate;
    [SerializeField] float sightRange;

    
    private bool inSightRange;
    private bool bounce;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 initialVelocity = new Vector3().Randomize(-100f, 100f);
        initialVelocity = initialVelocity.normalized * speed/100;
        rb.velocity = initialVelocity;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        inSightRange = distanceFromPlayer < sightRange;
        if (!inSightRange) Idle();
        if (inSightRange) Chase();

        if (rb.velocity.magnitude > speed * Time.fixedDeltaTime)
        {
            rb.velocity -= rb.velocity * dampeningRate * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = collision.contacts[0].normal * bounceSpeed + (Vector3.up * bounceSpeed/3);
    }


    void Idle()
    {
        Vector3 deviation = new Vector3().Randomize(-speed * Time.fixedDeltaTime, speed * Time.fixedDeltaTime);
        rb.AddForce(deviation * speed * Time.fixedDeltaTime / 2, ForceMode.Acceleration);
    }
    private void Chase()
    {
        rb.AddForce((player.centerPos - transform.position) * speed * Time.fixedDeltaTime, ForceMode.Acceleration);

        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    void Bounce()
    {
        
    }
}
