using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineSLS;

public class BossOne : EnemyBase
{
    public UnityEngine.AI.NavMeshAgent enemy;
    public LayerMask whatIsGround, whatIsPlayer;
    public float timeBetweenAttacks;
    public float attackRange;
    public bool playerInAttackRange;
    ObjectPool pool;

    float timeLeftBeforeAttack;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<UnityEngine.AI.NavMeshAgent>();
        pool = GetComponent<ObjectPool>();
        timeLeftBeforeAttack = timeBetweenAttacks;


        stateMachine = new BossStateMachine(this);
    }
    
    private void Update()
    {
        stateMachine.Update();




    } 

    //State Machine

    private BossStateMachine stateMachine;
    public class BossStateMachine : StateMachine
    {
        public BossStateMachine(MonoBehaviour owner) : base(owner) { }

        public new enum State
        {
            Idle,
            Tornado,
            Guarding,
            Stunned
        }

        protected override void InitializeStates()
        {
            RegisterState(new IdleState(owner));
            RegisterState(new SwirlState(owner));
            RegisterState(new GuardingState(owner));
            RegisterState(new StunnedState(owner));
        }


        public class BossStateBase : StateBase
        {
            public BossStateBase(MonoBehaviour owner) : base(owner)
            {
                //transform = trans;
                //playerTransform = player;
            }

            protected Transform transform;
            protected Transform playerTransform;










        }




        public class IdleState : BossStateBase
        {
            public IdleState(MonoBehaviour owner) : base(owner) { }


            public override void Update()
            {
                transform.LookAt(playerTransform.position);




            }


        }
        public class SwirlState : BossStateBase
        {
            public SwirlState(MonoBehaviour owner) : base(owner) { }
        }

        public class GuardingState : BossStateBase
        {
            public GuardingState(MonoBehaviour owner) : base(owner) { }
        }

        public class StunnedState : BossStateBase
        {
            public StunnedState(MonoBehaviour owner) : base(owner) { }
        }




    }



















    void OnDestroy()
    {
        stateMachine.Cleanup();
    }

    /*
    
    
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
