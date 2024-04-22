using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3Helper;

public class BossOctopus : EnemyBase
{
    //Config
    public bool on;

    [SerializeField] float rubbleFallRate;
    [SerializeField] GameObject bigRubble;
    [SerializeField] int bigRubbleFallRate;
    [SerializeField] BoxCollider rubbleSpawnArea;
    //[SerializeField] Vector2 tentacleAttackRate;
    //[SerializeField] OctopusBossTentacle[] arms;
    [SerializeField] float faceDamageMult = 10;


    //Connections
    ObjectPool rubblePool;
    private Transform thisTransform;
    private Health health;
    Animator anim;


    //Data
    float rubbleSpawnTimer;
    float bigRubbleSpawnProgress;
    float tentacleAttackTimer;




    protected override void Awake()
    {
        base.Awake();
        thisTransform = transform;
        health = GetComponent<Health>();
        rubblePool = GetComponent<ObjectPool>();
        anim = GetComponent<Animator>();
        //tentacleAttackTimer = Random.Range(tentacleAttackRate.x, tentacleAttackRate.y);
        if (on) TURNON();
    }

    public void TURNON()
    {
        on = true;
        HUDUIManager.i.ActivateBossSection();
    }

    private void Update()
    {
        if (!on) return;

        if (rubbleSpawnTimer < rubbleFallRate) rubbleSpawnTimer += Time.deltaTime;
        else
        {
            if(bigRubbleSpawnProgress >= bigRubbleFallRate && !bigRubble.activeSelf)
            {
                bigRubbleSpawnProgress = Random.Range(-1, 2);

                bigRubble.SetActive(true);

                Position basePosition = thisTransform.position + rubbleSpawnArea.center;
                Position offsetFactor = Vector3.zero.Randomize(-1, 1); offsetFactor.z = -1; offsetFactor.x /= 2;
                bigRubble.transform.position = basePosition + offsetFactor * (rubbleSpawnArea.size / 2);

            }
            else
            {
                PoolableObject rub;
                Position basePosition = thisTransform.position + rubbleSpawnArea.center;
                Position offsetFactor = Vector3.zero.Randomize(-1, 1);

                bigRubbleSpawnProgress++;
                rub = rubblePool.Pump();

                rub.Prepare_Basic(basePosition + offsetFactor * (rubbleSpawnArea.size / 2), Vector3.zero.Randomize(0, 360), Vector3.zero);
            }
            rubbleSpawnTimer = 0;
        }

        /*
        if (tentacleAttackTimer > 0) tentacleAttackTimer -= Time.deltaTime;
        else
        {
            tentacleAttackTimer = Random.Range(tentacleAttackRate.x, tentacleAttackRate.y);
            OctopusBossTentacle arm = GetTentacleToUse();
            arm.BeginAttack(Random.value > 0.5f);
        }
         */


    }
    /*
    OctopusBossTentacle GetTentacleToUse()
    {
        OctopusBossTentacle result = arms[0];

        result = arms[Random.Range(0, arms.Length)];

        
        int antiFreeze = 0;
        while (result.dead || result.attacking)
        {
            result = arms[Random.Range(0, arms.Length)];
            if (antiFreeze == 200) break;
            antiFreeze++;
        }
         
        return result;
    }
     */




    public void OnHealthChange(Health.Interaction args)
    {
        Debug.Log("test");

        if(args.type == Health.DamageType.Melee)
        {
            if(args.customIdentifier != "TentacleDamaged")
            {
                args.amount *= (int)faceDamageMult;
                bigRubble.SetActive(false);
                anim.SetTrigger("Attacked2");
            }
            else
            {
                args.amount *= 10;
                anim.SetTrigger("Attacked1");
            }
        }
        if (args.type == Health.DamageType.Projectile) args.amount /= 2;
    }
}
