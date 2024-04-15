using UnityEngine;
using UnityEngine.AI;
using Vector3Helper;

public class EnemyRanged : EnemyBase
{
    //Config
    public LayerMask whatIsGround, whatIsPlayer;
    public float timeBetweenAttacks;
    public float sightRange, attackRange;
    private bool inSightRange, inAttackRange;
    public float throwSpeed = 15f;
    [SerializeField] private ThrowingSystem thrower;

    //Connections
    public NavMeshAgent navAgent;
    private ObjectPool pool;
    private Animator anim;

    //Data
    private float timeLeftBeforeAttack;

    protected override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();
        pool = GetComponent<ObjectPool>();
        timeLeftBeforeAttack = timeBetweenAttacks;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        inSightRange = distanceFromPlayer < sightRange;
        inAttackRange = distanceFromPlayer < attackRange;

        if (!inSightRange && !inAttackRange) Idle();
        if (inSightRange && !inAttackRange) ChasePlayer();
        if (inSightRange && inAttackRange) AttackPlayer();

        anim.SetFloat("Speed", navAgent.velocity.magnitude);
    }

    private void Idle()
    {

    }

    private void ChasePlayer() => navAgent.SetDestination(player.position);

    private void AttackPlayer()
    {
        navAgent.SetDestination(transform.position);
        transform.LookAt((player.position * Direction.XZ) + (Direction.up * transform.position.y));

        if (timeLeftBeforeAttack > 0)
        {
            timeLeftBeforeAttack -= Time.deltaTime;
        }
        else
        {
            anim.SetTrigger("Attack");

            timeLeftBeforeAttack = timeBetweenAttacks;
        }

    }

    public void AttackCallback()
    {
        audio.PlaySound("Attack");
        PoolableObject bullet = pool.Pump();
        Direction direction = player.centerPos - transform.position;
        float power = thrower.Throw(direction);
        bullet.Prepare_Basic(transform.position, Vector3.zero, direction.Rotate(-thrower.angle, transform.right).normalized * power);
    }

}

