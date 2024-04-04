using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSLS;
using Vector3Helper;

public class BossOne : EnemyBase
{
    //Config
    public bool on;
    [SerializeField] Transform          thisTransform;
    [SerializeField] Transform          playerTransform;
    [SerializeField] AudioCaller        audio;
    [SerializeField] Health             health;
    [SerializeField] float              idleAttackRate;
    [SerializeField] float              idleAttackSpread;
    [SerializeField] float              idleAttackVelocity;
    [SerializeField] ObjectPool         idleProjPool;
    [SerializeField] float              spinactivationTime;
    [SerializeField] float              spinactivationRange;
    [SerializeField] AttackHitCollider  contactDamage;
    [SerializeField] float              stateSwitchRate;
    [SerializeField] float              stunTime;
    [SerializeField] float              guardAttackRate;
    [SerializeField] float              guardAttackVelocity;
    [SerializeField] float              guardAttackAngle;
    [SerializeField] ObjectPool         guardProjPool;




    private void Awake()
    {
        stateMachine = BossStateMachine.Create<BossStateMachine>(this);
        FeedDataToMachine1();
    }
    
    private void Update()
    {
        if (!on) return;
        stateMachine.Update();
    }

    public void FeedDataToMachine1()
    {
        stateMachine.thisTransform       = thisTransform;
        stateMachine.playerTransform     = playerTransform;
        stateMachine.audio               = audio;
        stateMachine.health              = health;
        stateMachine.idleAttackRate      = idleAttackRate;
        stateMachine.idleAttackSpread    = idleAttackSpread;
        stateMachine.idleAttackVelocity  = idleAttackVelocity;
        stateMachine.idleProjPool        = idleProjPool;
        stateMachine.spinactivationTime  = spinactivationTime;
        stateMachine.spinactivationRange = spinactivationRange;
        stateMachine.contactDamage       = contactDamage;
        stateMachine.stateSwitchRate     = stateSwitchRate;
        stateMachine.stunTime            = stunTime;
        stateMachine.guardAttackRate     = guardAttackRate;
        stateMachine.guardAttackVelocity = guardAttackVelocity;
        stateMachine.guardAttackAngle    = guardAttackAngle;
        stateMachine.guardProjPool       = guardProjPool;
    }

    //State Machine

    private BossStateMachine stateMachine;
    public class BossStateMachine : StateMachine
    {

        public new enum State
        {
            Idle,
            Spin,
            Guarding,
            Stunned
        }

        protected override void InitializeStates()
        {
            RegisterState<IdleState>();
            RegisterState<SwirlState>();
            RegisterState<GuardingState>();
            RegisterState<StunnedState>();
        }


        #region Special Data
        public Transform thisTransform;
        public Transform playerTransform;
        public AudioCaller audio;
        public Health health;
        public float idleAttackRate;
        public float idleAttackSpread;
        public float idleAttackVelocity;
        public ObjectPool idleProjPool;
        public float spinactivationTime;
        public float spinactivationRange;
        public AttackHitCollider contactDamage;
        public float stateSwitchRate;
        public float stunTime;
        public float guardAttackRate;
        public float guardAttackVelocity;
        public float guardAttackAngle;
        public ObjectPool guardProjPool;


        #endregion

        public class BossStateBase : StateBase { public BossStateMachine M => (BossStateMachine)base.machine; }


        public class IdleState : BossStateBase
        {
            float attackTimer;
            float spinTimer;
            float changeTimer;

            public override void Update()
            {
                M.thisTransform.LookAt(M.playerTransform.position);
                float distance = Vector2.Distance(M.playerTransform.position, M.thisTransform.position);

                if (attackTimer < M.idleAttackRate) attackTimer += Time.deltaTime;
                if (attackTimer >= M.idleAttackRate)
                {
                    attackTimer = 0f;

                    Vector3 baseDirection = (M.playerTransform.position - M.thisTransform.position) * Direction.XZ;

                    PoolableObject proj1 = M.idleProjPool.Pump();
                    PoolableObject proj2 = M.idleProjPool.Pump();
                    PoolableObject proj3 = M.idleProjPool.Pump();
                    PoolableObject proj4 = M.idleProjPool.Pump();
                    PoolableObject proj5 = M.idleProjPool.Pump();

                    proj1.Prepare_Basic(M.thisTransform.position, Vector3.zero, baseDirection);
                    proj2.Prepare_Basic(M.thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(M.idleAttackSpread, Vector3.up));
                    proj3.Prepare_Basic(M.thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(M.idleAttackSpread/2, Vector3.up));
                    proj4.Prepare_Basic(M.thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(-M.idleAttackSpread, Vector3.up));
                    proj5.Prepare_Basic(M.thisTransform.position, Vector3.zero, baseDirection.Direction().Rotate(-M.idleAttackSpread/2, Vector3.up));
                }

                if (distance < M.spinactivationRange)
                {
                    if(spinTimer < M.spinactivationTime) spinTimer += Time.deltaTime;
                    else
                    {
                        M.ChangeState((int)BossStateMachine.State.Spin);
                        spinTimer = 0;
                    }
                }
                else if (spinTimer > 0) spinTimer = 0;

                if(changeTimer < M.stateSwitchRate) changeTimer += Time.deltaTime;
                else
                {
                    changeTimer = 0;
                    M.ChangeState((int)BossStateMachine.State.Guarding);
                }

            }

            public override void OnEnterState()
            {
                changeTimer = 0;
            }
        }
        public class SwirlState : BossStateBase
        {

            public override void OnEnterState()
            {
                M.contactDamage.enabled = true;
            }
            public override void OnExitState()
            {
                M.contactDamage.enabled = false;
            }
        }
        public class GuardingState : BossStateBase
        {
            float attackTimer;
            float changeTimer;

            public override void Update()
            {
                M.thisTransform.LookAt(M.playerTransform.position);

                if (attackTimer < M.guardAttackRate) attackTimer += Time.deltaTime;
                else
                {
                    attackTimer = 0;
                    var proj = M.guardProjPool.Pump();

                    proj.Prepare_Basic(M.thisTransform.position, Vector3.zero, 
                        ((M.playerTransform.position - M.thisTransform.position) 
                        * Direction.XZ).Rotate(M.guardAttackAngle, M.thisTransform.right) 
                        * M.guardAttackVelocity
                        );
                }

                if (changeTimer < M.stateSwitchRate) changeTimer += Time.deltaTime;
                else
                {
                    changeTimer = 0;
                    M.ChangeState((int)BossStateMachine.State.Guarding);
                }
            }

            public override void OnEnterState()
            {
                M.health.damagable = false;
                changeTimer = 0;
            }
            public override void OnExitState()
            {
                M.health.damagable = true;
            }
        }
        public class StunnedState : BossStateBase
        {
            float timeLeft;

            public override void Update()
            {
                if(timeLeft < M.stunTime) timeLeft += Time.deltaTime;
                else
                {
                    timeLeft = 0;
                    M.ChangeState((int)BossStateMachine.State.Idle);
                }
            }
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

    void OnDeplete()
    {
        GetComponent<LootBag>().DropLoot(transform.position);
        Destroy(gameObject);
    }
     */
}
