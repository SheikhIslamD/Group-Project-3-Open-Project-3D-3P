using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerShooter))]
public class PlayerMelee : MonoBehaviour
{
    GameplayInputReader input;
    PlayerShooter shooter;
    [SerializeField] float slashDistance;
    [SerializeField] float slashRadius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] int damage = 40;
    [SerializeField] GameObject slashEcho;


    private void Start()
    {
        input = GameplayInputReader.Get();
        shooter = GetComponent<PlayerShooter>();
        slashEcho.transform.localScale = Vector3.one * slashRadius;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 vagueDirection = new Vector3(shooter.aimDirection.x, 0, shooter.aimDirection.z).normalized;



        slashEcho.SetActive(input.melee.IsPressed());
        if (input.melee.IsPressed())
        {
            slashEcho.transform.position = transform.position + (slashDistance * vagueDirection);
            //Invoke("StopSlash", 0.1f);

            RaycastHit hit;
            Physics.SphereCast(transform.position, slashRadius - 1, slashDistance * vagueDirection, out hit, layerMask);

            if (hit.collider == null) return;

            Health health = hit.collider.GetComponent<Health>();
            if (health != null) health.Damage(damage, Health.DamageType.Melee);

            ReflectableProjectile reflect = hit.collider.GetComponent<ReflectableProjectile>();
            if (reflect != null)
            {
                Debug.Log("Hit Projectile");

                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                Vector3 direction = reflect.sender.position - rb.position;
                rb.AddForce(direction.normalized * 1400);
                reflect.MakeReflected();
            }
        }
    }

    void StopSlash() => slashEcho.SetActive(false);

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position + (slashDistance * new Vector3(shooter.aimDirection.x, 0, shooter.aimDirection.z).normalized), slashRadius);
    }

}
