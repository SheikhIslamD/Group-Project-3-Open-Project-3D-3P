using UnityEngine;
using Vector3Helper;

[RequireComponent(typeof(PoolableObject))]
public class ReflectableProjectile : MonoBehaviour
{
    public Transform sender => GetComponent<PoolableObject>().pool.transform;


    private ThrowingSystem thrower = new(20, 2000);
    private bool isReflected;

    public static bool Reflect(GameObject target)
    {
        ReflectableProjectile reflect = target.GetComponent<ReflectableProjectile>();
        if (!reflect) return false;

        reflect.MakeReflected();
        Rigidbody rb = reflect.GetComponent<Rigidbody>();

        Direction direction = reflect.sender.position - (rb.position + Vector3.up);
        float power = reflect.thrower.Throw(direction);
        rb.velocity = direction.Rotate(reflect.thrower.angle, Vector3.Cross(direction, Vector3.up)).normalized * (power + 6f);


        return true;
    }

    public void MakeReflected()
    {
        if (isReflected) return;

        gameObject.layer = Layers.PlayerProjectile;
        isReflected = true;
    }

    public void MakeNormal()
    {
        if (!isReflected) return;

        gameObject.layer = Layers.EnemyProjectile;
        isReflected = false;
    }

    private void OnCollisionEnter(Collision collision) => Collide(collision.gameObject);
    private void OnTriggerEnter(Collider other) => Collide(other.gameObject);

    private void Collide(GameObject obj)
    {
        if (isReflected && obj.GetComponent<Health>())
        {
            MakeNormal();
        }
    }

    private void OnDisable() => MakeNormal();
}
