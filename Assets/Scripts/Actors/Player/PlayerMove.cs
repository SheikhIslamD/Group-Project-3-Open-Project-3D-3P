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

    //Connections
    GameplayInputReader input;
    CharacterController characterController;
    Transform cameraTransform;
    AudioCaller audioC;
    Health health;
    PlayerShooter shooter;
    PlayerAnimator anim;
    
    //Data
    Vector3 movementDirection;
    float movementMagnitude;
    float ySpeed;
    float originalStepOffset;
    float dodgeTimeLeft;
    Vector3 dodgeDirection;
    [HideInInspector] public Vector3 movementVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        cameraTransform = Camera.main.transform;
        GameplayInputReader.Get(ref input);
        audioC = GetComponent<AudioCaller>();
        health = GetComponent<Health>();
        shooter = GetComponent<PlayerShooter>();
        anim = GetComponentInChildren<PlayerAnimator>();
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

            if (input.jump.WasPressedThisFrame() || input.jump.WasReleasedThisFrame())
            {
                audioC.PlaySound("Jump");
                ySpeed = jumpSpeed;
                anim.Jump();
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
        dodgeTimeLeft = dodgeTime;
        dodgeDirection = movementDirection;
        health.damagable = false;
        anim.Dodge();
    }


}