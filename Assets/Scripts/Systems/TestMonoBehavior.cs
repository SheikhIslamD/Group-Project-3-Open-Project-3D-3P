using System.Collections;
using UnityEngine;
using TrigHelper;
using Vector3Helper;
using System.Collections.Generic;

public class TestMonoBehavior : MonoBehaviour
{


    public Transform origin;
    public Transform target;
    public Rigidbody project;
    public float angle;
    public float max;
    float G = 9.81f;

    float timer;

    private void Update()
    {
        if (timer < 2f) timer += Time.deltaTime;
        else
        {
            timer = 0;
            Shoot();
        }

    }

    void Shoot()
    {
        Vector2 origin = this.origin.position;
        Vector2 target = this.target.position;
        Radian angle = this.angle.Degree().ToRadians();

        float X = target.x - origin.x;
        float Y = target.y - origin.y;
        float S = angle.Sin();
        float C = angle.Cos();
        float T = angle.Tan();

        float force = Mathf.Sqrt((
            (G * X.P())
            /
            (2 * C * (Y * C - S * X))
            ) * -1);
        if (float.IsNaN(force)) force = max;

        Debug.Log(force);
        project.Move(this.origin.position, Quaternion.identity);

        project.velocity = Direction.right.Rotate(angle.ToDegrees(), Direction.forward) * force;
    }

}
