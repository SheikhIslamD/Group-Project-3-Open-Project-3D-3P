using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneAnimator : MonoBehaviour
{
    Animator anim;
    BossOne boss;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        boss = GetComponentInParent<BossOne>();
    }


    public void BeginAttack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Attack");
    }
    public void SetGuarding(bool value) => anim.SetBool("Guarding", value);
    public void Stun(bool value)
    {
        anim.SetBool("Stun", value);
        anim.SetBool("Guarding", false);
    }


    public void AttackCallback() => boss.AttackCallback();










}
