using System;
using TrigHelper;
using UnityEngine;
using Vector3Helper;

[Serializable]
public struct ThrowingSystem
{
    public float angle;
    public float maxSpeed;
    public float gravity;

    public ThrowingSystem(float angle, float maxSpeed, float gravity = 9.81f)
    {
        this.angle = angle;
        this.maxSpeed = maxSpeed;
        this.gravity = gravity;
    }

    public float Throw(Direction vector)
    {
        float X = (vector.x.P() + vector.z.P()).SQRT();
        float Y = vector.y;

        Radian angle = this.angle.Degree().ToRadians();
        float S = angle.Sin();
        float C = angle.Cos();

        float force = Mathf.Sqrt(
            gravity * X.P()
            /
            (2 * C * ((Y * C) - (S * X)))
             * -1);
        if (float.IsNaN(force) || force > maxSpeed) force = maxSpeed;

        Debug.Log(force);

        return force;
    }





}