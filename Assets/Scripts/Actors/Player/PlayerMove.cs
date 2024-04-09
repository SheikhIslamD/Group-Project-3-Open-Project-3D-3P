using UnityEngine;
using UnityEngine.Events;
using Vector3Helper;

public class PlayerMove : MonoBehaviour
{
    //Parameters
    [SerializeField] public float speed = 6.0f;
    [SerializeField] float rotationSpeed = 720.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float dodgeSpeed = 12.0f;
    [SerializeField] float dodgeTime = 0.4f;
    [SerializeField] float deathBarrier = -15f;
    [SerializeField] int amountOfAirDodges = 1;

    //Connections
    GameplayInputReader input;
    CharacterController characterController;
    Transform cameraTransform;
    AudioCaller audioC;
    Health health;
    PlayerShooter shooter;
    PlayerAnimator anim;
    new BoxCollider collider;
    
    //Data
    Vector3 movementDirection;
    float movementMagnitude;
    float ySpeed;
    float originalStepOffset;
    float dodgeTimeLeft;
    Vector3 dodgeDirection;
    Vector3 lastGroundedPosition;
    bool onGround => characterController.isGrounded;
    int dodgesLeft;
    [HideInInspector] public Vector3 movementVelocity;
    [HideInInspector] public Vector3 position => transform.position;
    [HideInInspector] public Vector3 centerPos => transform.position + collider.center;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        cameraTransform = Camera.main.transform;
        GameplayInputReader.Get(ref input);
        audioC = GetComponent<AudioCaller>();
        health = GetComponent<Health>();
        shooter = GetComponent<PlayerShooter>();
        anim = GetComponentInChildren<PlayerAnimator>();
        collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        MovementDirection();

        Vector3 velocity;

        if (dodgeTimeLeft > 0)
        {
            dodgeTimeLeft -= Time.deltaTime;
            if (dodgeTimeLeft <= 0)
            {
                dodgeTimeLeft = 0;
                health.damagable = true;
            }

            velocity = DodgeMovement();
        }
        else
        {
            velocity = BasicMovement();
            AimBasedRotation();

            if (input.sprint.WasPressedThisFrame() && input.movementVector2 != Vector2.zero) BeginDodge();
        }

        movementVelocity = velocity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if(transform.position.y < deathBarrier)
        {
            characterController.Move(lastGroundedPosition - transform.position);
            GetComponent<Health>().Damage(25, Health.DamageType.Generic, this, "BottomlessPit");
        }

        anim.p_inAir = !characterController.isGrounded;
        if(onGround && dodgesLeft != amountOfAirDodges) dodgesLeft = amountOfAirDodges;
    }

    void MovementDirection()
    {
        movementDirection = new Vector3(input.movementVector2.x, 0, input.movementVector2.y);
        movementDirection = movementDirection.Direction().Rotate(cameraTransform.eulerAngles.y, Direction.up);

        movementMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();
    }

    Vector3 BasicMovement()
    {
        Vector3 velocity = movementDirection * movementMagnitude * speed;

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (input.jump.WasPressedThisFrame()) anim.Jump();
            if (jumping)
            {
                ySpeed = jumpSpeed;
                jumping = false;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        velocity.y = ySpeed;

        return velocity;
    }

    Vector3 DodgeMovement()
    {
        return dodgeDirection * dodgeSpeed;
    }

    void MovementBasedRotation()
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void AimBasedRotation()
    {
        Quaternion toRotation = Quaternion.LookRotation(shooter.aimDirection * Direction.XZ, transform.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    void BeginDodge()
    {
        if (!onGround && dodgesLeft < 1) return;
        dodgeTimeLeft = dodgeTime;
        dodgeDirection = movementDirection;
        health.damagable = false;
        anim.Dodge();
        dodgesLeft--;
    }

    public void StorePosition()
    {
        lastGroundedPosition = transform.position;
    }

    bool jumping;
    public void JumpCallback()
    {
        Debug.Log("Jump");
        audioC.PlaySound("Jump");
        jumping = true;
    }

}