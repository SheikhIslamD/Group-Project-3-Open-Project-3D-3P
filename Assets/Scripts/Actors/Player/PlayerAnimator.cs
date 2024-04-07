using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3Helper;

public class PlayerAnimator : MonoBehaviour
{
    //Config



    //Components
    Animator animator;
    PlayerMove movement;
    PlayerShooter shooter;
    PlayerMelee melee;
    Transform movementTransform;

    //Data
    float fullSpeed;

    //Animation Parameters
    float p_walkX { set { animator.SetFloat("walkX", value); } }
    float p_walkZ { set { animator.SetFloat("walkZ", value); } }
    void p_Dodge() => animator.SetTrigger("Dodge");
    void p_Melee() => animator.SetTrigger("Melee");
    void p_Throw() => animator.SetTrigger("Throw");
    void p_Jump() => animator.SetTrigger("Jump");
    void p_Cook() => animator.SetTrigger("Cook");



    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMove>();
        shooter = GetComponentInParent<PlayerShooter>();
        melee = GetComponentInParent<PlayerMelee>();
        movementTransform = movement.transform;
        fullSpeed = movement.speed;
    }

    private void Update()
    {
        SetDirection();
    }

    void SetDirection()
    {

        Vector3 result = movementTransform.worldToLocalMatrix * (Vector3)(movement.movementVelocity * Direction.XZ / (fullSpeed * Time.deltaTime));

        p_walkX = result.x;
        p_walkZ = result.z;

        //Neither of these worked and I honestly have no clue why
        //Vector3 result = ((Direction)moveDirection).RotateTo(aimDirection, Direction.front);
        //Direction result = Quaternion.LookRotation(aimDirection, transform.up) * moveDirection;
    }

    public void Dodge() => p_Dodge();
    public void Melee() => p_Melee();
    public void Throw() => p_Throw();
    public void Jump() => p_Jump();
    public void Cook() => p_Cook();

    public void ThrowCallback()
    {
        shooter.KnifeCallback();
    }
    public void CookCallback()
    {

    }

}
