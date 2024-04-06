using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSLS;
using Vector3Helper;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(SphereCollider), typeof(Health), typeof(AttackHitCollider))]
[RequireComponent(typeof(Pauseable), typeof(ObjectPool))]
public class BossOne : EnemyBase
{
    //Config
    public bool on; public void TURNON() => on = true;
    [SerializeField] float idleAttackRate;
    [SerializeField] float idleAttackSpread;
    [SerializeField] float idleAttackVelocity;
    [SerializeField] ObjectPool idleProjPool;
    [SerializeField] float spinactivationTime;
    [SerializeField] float spinactivationRange;
    [SerializeField] float stateSwitchRate;
    [SerializeField] float stunTime;
    [SerializeField] float guardAttackRate;
    [SerializeField] float guardAttackVelocity;
    [SerializeField] float guardAttackAngle;
    [SerializeField] ObjectPool guardProjPool;

    //Connections
    Transform thisTransform;
    Transform playerTransform;
    Health health;
    AttackHitCollider contactDamage;

    //Data
    public States currentStateVisual;


    protected override void Start()
    {
        base.Start();
        thisTransform = transform;
        playerTransform = GameObject.Find("Player").transform;
        health = GetComponent<Health>();
        contactDamage = GetComponent<AttackHitCollider>();

        stateMachine = StateMachine.Create(this);
        stateMachine.owner = this;
    }
    
    private void Update()
    {
        if (!on) return;
        stateMachine.Update();
        currentStateVisual = stateMachine.currentStateID;
    }

    public enum States
    {
        Idle,
        Swirling,
        Guarding,
        Stunned
    }

    //State Machine

    private StateMachine stateMachine;
    public class StateMachine : StateMachine<BossOne, StateMachine>
    {



        public new States currentStateID => (States)base.currentStateID;

        public StateBase ChangeState(States state) => base.ChangeState((int)state);

        protected override void InitializeStates()
        {
            RegisterState<IdleState>();
            RegisterState<SwirlState>();
            RegisterState<GuardingState>();
            RegisterState<StunnedState>();
        }




        public class IdleState : StateBase
        {
            float attackTimer;
            float spinTimer;
            float changeTimer;

            public override void Update()
            {
                owner.thisTransform.LookAt(owner.playerTransform.position);
                float distance = Vector2.Distance(owner.playerTransform.position, owner.thisTransform.position);

                if (attackTimer < owner.idleAttackRate) attackTimer += Time.deltaTime;
                if (attackTimer >= owner.idleAttackRate)
                {
                    attackTimer = 0f;
                    owner.IdleAttack();
                }

                if (distance < owner.spinactivationRange)
                {
                    if(spinTimer < owner.spinactivationTime) spinTimer += Time.deltaTime;
                    else
                    {
                        //M.ChangeState(States.Swirling);
                        spinTimer = 0;
                    }
                }
                else if (spinTimer > 0) spinTimer = 0;

                if(changeTimer < owner.stateSwitchRate) changeTimer += Time.deltaTime;
                else
                {
                    changeTimer = 0;
                    machine.ChangeState(States.Guarding);
                }

            }

            public override void OnEnterState()
            {
                changeTimer = 0;
            }
        }
        public class SwirlState : StateBase
        {

            public override void OnEnterState()
            {
                owner.contactDamage.enabled = true;
            }
            public override void OnExitState()
            {
                owner.contactDamage.enabled = false;
            }
        }
        public class GuardingState : StateBase
        {
            float attackTimer;
            float changeTimer;

            public override void Update()
            {
                owner.thisTransform.LookAt(owner.playerTransform.position);

                if (attackTimer < owner.guardAttackRate) attackTimer += Time.deltaTime;
                else
                {
                    attackTimer = 0;
                    owner.GuardAttack();
                }

                if (changeTimer < owner.stateSwitchRate) changeTimer += Time.deltaTime;
                else
                {
                    changeTimer = 0;
                    machine.ChangeState((int)States.Idle);
                }
            }

            public override void OnEnterState()
            {
                owner.health.damagable = false;
                changeTimer = 0;
            }
            public override void OnExitState()
            {
                owner.health.damagable = true;
            }
        }
        public class StunnedState : StateBase
        {
            float timeLeft;

            public override void Update()
            {
                if(timeLeft < owner.stunTime) timeLeft += Time.deltaTime;
                else
                {
                    timeLeft = 0;
                    machine.ChangeState((int)States.Idle);
                }
            }
        }
    }
    
    
    
    private void IdleAttack()
    {
        Vector3 baseDirection = (playerTransform.position - thisTransform.position) * Direction.XZ;

        PoolableObject proj1 = idleProjPool.Pump();
        PoolableObject proj2 = idleProjPool.Pump();
        PoolableObject proj3 = idleProjPool.Pump();
        PoolableObject proj4 = idleProjPool.Pump();
        PoolableObject proj5 = idleProjPool.Pump();

        proj1.Prepare_Basic(thisTransform.position, Vector3.zero, baseDirection);
        proj2.Prepare_Basic(thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(idleAttackSpread, Vector3.up));
        proj3.Prepare_Basic(thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(idleAttackSpread / 2, Vector3.up));
        proj4.Prepare_Basic(thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(-idleAttackSpread, Vector3.up));
        proj5.Prepare_Basic(thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(-idleAttackSpread / 2, Vector3.up));
    }
    
    private void GuardAttack()
    {
        var proj = guardProjPool.Pump();

        proj.Prepare_Basic(thisTransform.position, Vector3.zero,
            ((playerTransform.position - thisTransform.position)
            * Direction.XZ).Rotate(-guardAttackAngle, thisTransform.right)
            * guardAttackVelocity * (Vector3.Distance(playerTransform.position, thisTransform.position) / 5)
            );
    }














    protected void OnHealthChange(Health.Interaction args)
    {
        if(stateMachine.currentStateID == States.Guarding && args.source.GetComponent<ReflectableProjectile>())
        {
            Debug.Log("Stunned");
            args.interrupted = false;
            stateMachine.ChangeState(States.Stunned);
        }
    }


    void OnDestroy()
    {
        stateMachine.Cleanup();
    }

    /*
        public UnityEngine.AI.NavMeshAgent enemy;
        public LayerMask whatIsGround, whatIsPlayer;
        public float timeBetweenAttacks;
        public float attackRange;
        public bool playerInAttackRange;
        ObjectPool pool;

    float timeLeftBeforeAttack;
    
            player = GameObject.Find("Player").transform;
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        pool = GetComponent<ObjectPool>();
        timeLeftBeforeAttack = timeBetweenAttacks;

        private void Update()
    {
        




        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }
    
    
    
    
    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        enemy.SetDestination(transform.position);
        transform.LookAt(player);

        if(timeLeftBeforeAttack > 0)
        {
            timeLeftBeforeAttack -= Time.deltaTime;
        }
        else
        {
            PoolableObject sphere = pool.Pump();
            sphere.Prepare_Basic(transform.position, Vector3.zero, transform.forward * 32f + transform.up * 2f);

            /*
            sphere.gameObject.SetActive(true);
            sphere.transform.position = transform.position;
            sphere.transform.eulerAngles = Vector3.zero;
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
             

    timeLeftBeforeAttack = timeBetweenAttacks;
        }

    }

    void OnHealthChange(Health.DamageArgs args)
    {
        GetComponent<LootBag>().DropLoot(transform.position);
        Destroy(gameObject);
    }
     */
}
