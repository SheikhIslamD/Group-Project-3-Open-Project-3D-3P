using StateMachineSLS;
using Unity.VisualScripting;
using UnityEngine;
using Vector3Helper;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Pauseable), typeof(ObjectPool))]
public class BossOne : EnemyBase
{
    //Config
    public bool on;
    [SerializeField] private float idleAttackRate;
    [SerializeField] private float idleAttackSpread;
    [SerializeField] private float idleAttackVelocity;
    [SerializeField] private ObjectPool idleProjPool;
    [SerializeField] private float spinactivationTime;
    [SerializeField] private float spinactivationRange;
    [SerializeField] private float stateSwitchRate;
    [SerializeField] private float stunTime;
    [SerializeField] private float guardAttackRate;
    [SerializeField] private ObjectPool guardProjPool;
    [SerializeField] private ThrowingSystem heavyThrower;
    [SerializeField] private Transform pearlSpawnPoint;
    public static bool nearBoss = false;

    //Connections
    private Transform thisTransform;
    private Health health;
    private AttackHitCollider contactDamage;
    private BossOneAnimator anim;

    //Data
    private States currentState => stateMachine.currentStateID;


    protected override void Awake()
    {
        base.Awake();
        thisTransform = transform;
        health = GetComponent<Health>();
        contactDamage = GetComponent<AttackHitCollider>();
        anim = GetComponentInChildren<BossOneAnimator>();

        stateMachine = StateMachine.Create(this);
        stateMachine.owner = this;
    }

    public void TURNON()
    {
        on = true;
        HUDUIManager.i.ActivateBossSection();
    }

    private void Update()
    {
        if (!on) return;
        stateMachine.Update();
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
            private float attackTimer;
            private float spinTimer;
            private float changeTimer;

            public override void Update()
            {
                owner.thisTransform.LookAt(owner.player.position * Direction.XZ);
                float distance = Vector2.Distance(owner.player.position, owner.thisTransform.position);

                if (attackTimer < owner.idleAttackRate) attackTimer += Time.deltaTime;
                if (attackTimer >= owner.idleAttackRate)
                {
                    attackTimer = 0f;
                    owner.anim.BeginAttack();
                }

                if (distance < owner.spinactivationRange)
                {
                    if (spinTimer < owner.spinactivationTime) spinTimer += Time.deltaTime;
                    else
                    {
                        //M.ChangeState(States.Swirling);
                        spinTimer = 0;
                    }
                }
                else if (spinTimer > 0) spinTimer = 0;

                if (changeTimer < owner.stateSwitchRate) changeTimer += Time.deltaTime;
                else
                {
                    changeTimer = 0;
                    machine.ChangeState(States.Guarding);
                    owner.anim.SetGuarding(true);
                }

            }

            public override void OnEnterState()
            {
                attackTimer = 0;
                changeTimer = 0;
            }
        }
        public class SwirlState : StateBase
        {

            public override void OnEnterState() => owner.contactDamage.enabled = true;
            public override void OnExitState() => owner.contactDamage.enabled = false;
        }
        public class GuardingState : StateBase
        {
            private float attackTimer;
            private float changeTimer;

            public override void Update()
            {
                owner.thisTransform.LookAt(owner.player.position * Direction.XZ);

                if (attackTimer < owner.guardAttackRate) attackTimer += Time.deltaTime;
                else
                {
                    attackTimer = 0;
                    owner.anim.BeginAttack();
                }

                if (changeTimer < owner.stateSwitchRate) changeTimer += Time.deltaTime;
                else
                {
                    changeTimer = 0;
                    machine.ChangeState(States.Idle);
                    owner.anim.SetGuarding(false);
                }
            }

            public override void OnEnterState()
            {
                owner.health.damagable = false;
                changeTimer = 0;
                attackTimer = 0;
            }
            public override void OnExitState() => owner.health.damagable = true;
        }
        public class StunnedState : StateBase
        {
            private float timeLeft;

            public override void Update()
            {
                if (timeLeft < owner.stunTime) timeLeft += Time.deltaTime;
                else
                {
                    timeLeft = 0;
                    machine.ChangeState((int)States.Idle);
                    owner.anim.Stun(false);
                }
            }
        }
    }


    public void AttackCallback()
    {
        if (currentState == States.Idle)
        {
            Vector3 baseDirection = player.centerPos - pearlSpawnPoint.position;

            PoolableObject proj1 = idleProjPool.Pump();
            PoolableObject proj2 = idleProjPool.Pump();
            PoolableObject proj3 = idleProjPool.Pump();
            PoolableObject proj4 = idleProjPool.Pump();
            PoolableObject proj5 = idleProjPool.Pump();

            proj1.Prepare_Basic(pearlSpawnPoint.position, Vector3.zero, baseDirection);
            proj2.Prepare_Basic(pearlSpawnPoint.position, Vector3.zero, baseDirection.Direction().Rotate(idleAttackSpread, Vector3.up));
            proj3.Prepare_Basic(pearlSpawnPoint.position, Vector3.zero, baseDirection.Direction().Rotate(idleAttackSpread / 2, Vector3.up));
            proj4.Prepare_Basic(pearlSpawnPoint.position, Vector3.zero, baseDirection.Direction().Rotate(-idleAttackSpread, Vector3.up));
            proj5.Prepare_Basic(pearlSpawnPoint.position, Vector3.zero, baseDirection.Direction().Rotate(-idleAttackSpread / 2, Vector3.up));
        }
        else if (currentState == States.Guarding)
        {
            PoolableObject proj = guardProjPool.Pump();

            Direction direction = player.centerPos - transform.position;
            float power = heavyThrower.Throw(direction);
            proj.Prepare_Basic(thisTransform.position, Vector3.zero, direction.Rotate(-heavyThrower.angle, transform.right).normalized * power);

        }


    }

    protected void OnHealthChange(Health.Interaction args)
    {
        if (stateMachine.currentStateID == States.Guarding && args.source.GetComponent<ReflectableProjectile>())
        {
            args.interrupted = false;
            stateMachine.ChangeState(States.Stunned);
            anim.Stun(true);
        }
    }

    private void OnDestroy() => stateMachine.Cleanup();
}
