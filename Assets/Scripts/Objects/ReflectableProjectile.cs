using UnityEngine;

[RequireComponent(typeof(PoolableObject))]
public class ReflectableProjectile : MonoBehaviour
{
    public Transform sender => GetComponent<PoolableObject>().pool.transform;

    private bool isReflected;

    public static bool Reflect(GameObject target)
    {
        ReflectableProjectile reflect = target.GetComponent<ReflectableProjectile>();
        if (reflect == null) return false;

        reflect.MakeReflected();
        Rigidbody rb = reflect.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Vector3 direction = (reflect.sender.position + (Vector3.up*2)) - rb.position;
        rb.AddForce(direction.normalized * 1600);
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
        if (isReflected && obj.GetComponent<Health>() != null)
        {
            MakeNormal();
        }
    }

    private void OnDisable() => MakeNormal();
}
