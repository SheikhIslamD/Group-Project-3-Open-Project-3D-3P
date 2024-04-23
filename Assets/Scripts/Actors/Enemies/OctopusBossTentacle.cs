using System.Collections;
using UnityEngine;
using Vector3Helper;


public class OctopusBossTentacle : EnemyBase
{
    //Config
    [SerializeField] bool active;
    [SerializeField] Health baseHealth;
    [SerializeField] float rotationSpeed = 720;
    [SerializeField] Vector2 appearTimeRange = new(2.5f, 15f);
    [SerializeField] Vector2 firstAttackTimeRange = new(1, 4);
    [SerializeField] Vector2 attackTimeRange = new(0, 3);

    [Header("Animation Delay Times")]
    [SerializeField] float timeBeforeDoingAnything = 0.5f;
    [SerializeField] float attackLongTime = 1.31f;
    [SerializeField] float attackQuickTime = 0.35f;

    //Connections
    Health health;
    Animator anim;

    //Data
    int attacksLeft;
    bool visibility;
    bool aiming = true;
    bool dead;

    float timer;


    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        Retreat();
    }

    private void Update()
    {
        if(dead || !active || !player) return;
        if (aiming) Aim();
        if (timer > 0) timer -= Time.deltaTime;
        else TimerStrike();
    }

    void TimerStrike()
    {
        timer = 0;
        if (!visibility)
        {
            SetVisibility(true);
            attacksLeft = Random.Range(1, 4);

            timer += timeBeforeDoingAnything;
            timer += RandomFV(firstAttackTimeRange);
        }
        else if (attacksLeft > 0)
        {
            BeginAttack();
            timer += (attacksLeft > 1) ? attackQuickTime : attackLongTime;
            timer += RandomFV(attackTimeRange);
        }
        else Retreat();

    }

    void Aim()
    {
        Direction dir = (player.transform.position - transform.position) * Direction.XZ;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir, transform.up), rotationSpeed * Time.deltaTime);
    }



    private void SetVisibility(bool visible)
    {
        visibility = visible;
        anim.SetBool("Visible", visible);
    }

    private void BeginAttack() => anim.SetTrigger("Attack");

    private void SetAttackNumber() => anim.SetInteger("AttacksLeft", attacksLeft);

    private void CounterAttacked()
    {
        anim.SetTrigger("Hurt");
        Retreat();
    }

    private void Retreat()
    {
        SetVisibility(false);
        timer = 0;
        timer += timeBeforeDoingAnything;
        timer += RandomFV(appearTimeRange);
    }

    float RandomFV(Vector2 input) => Random.Range(input.x, input.y);

    public void DecrementAttacks()
    {
        attacksLeft--;
        SetAttackNumber();
    }
    public void ToggleAiming() => aiming.Toggle();
    public void TURNON() => active = true;


    /*

    public void BeginAttack(bool counterClockwise)
    {
        attacking = true;
        anim.SetBool("CounterClockwise", counterClockwise);
        anim.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        attacking = false;
        anim.ResetTrigger("Attack");
    }
     */

    public void OnHealthChange(Health.Interaction args)
    {
        if(args.type != Health.DamageType.Melee)
        {
            args.Interrupt();
            return;
        }

        args.customIdentifier = "TentacleDamaged";
        baseHealth.ChangeHealth(args);
        CounterAttacked();

        if (args.depletes) dead = true;
    }

    //protected override void OnHealthDeplete() => dead = true;
}
