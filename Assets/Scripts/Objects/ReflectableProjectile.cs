using UnityEngine;

[RequireComponent(typeof(PoolableObject))]
public class ReflectableProjectile : MonoBehaviour
{
    public Transform sender => GetComponent<PoolableObject>().pool.transform;
    [SerializeField] LayerMask normalMask;
    [SerializeField] LayerMask reflectedMask;
    bool isReflected;

    public void MakeReflected()
    {
        var col = GetComponent<Collider>();
        col.includeLayers = reflectedMask;
        col.excludeLayers = ~reflectedMask;
        isReflected = true;
    }

    public void MakeNormal()
    {
        var col = GetComponent<Collider>();
        col.includeLayers = normalMask;
        col.excludeLayers = ~normalMask;
        isReflected = false;
    }

    private void OnCollisionEnter(Collision collision) => Collide(collision.gameObject);
    private void OnTriggerEnter(Collider other) => Collide(other.gameObject);
    void Collide(GameObject obj)
    {
        if (isReflected && obj.GetComponent<Health>() != null)
        {
            MakeNormal();
        }
    }
}
