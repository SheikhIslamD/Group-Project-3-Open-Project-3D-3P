using UnityEngine;
using Vector3Helper;

public class PlayerMove : MonoBehaviour
{
    //Parameters
    [SerializeField] public float acceleration = 2.0f;
    [SerializeField] public float speed = 6.0f;
    [SerializeField] private float rotationSpeed = 720.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float dodgeSpeed = 12.0f;
    [SerializeField] private float dodgeTime = 0.4f;
    [SerializeField] private float deathBarrier = -15f;
    [SerializeField] private int amountOfAirDodges = 1;
    [SerializeField] private LayerMask groundLayers;

    //Connections
    private GameplayInputReader input;
    private Rigidbody rb;
    private Transform cameraTransform;
    private AudioCaller audioC;
    private Health health;
    private PlayerShooter shooter;
    private PlayerAnimator anim;
    private new CapsuleCollider collider;
    private SphereCollider colliderFoot;

    //Data
    private Vector3 movementDirection;
    Vector3 nextVelocity;
    private float dodgeTimeLeft;
    private Vector3 dodgeDirection;
    public Vector3 lastGroundedPosition;
    private int dodgesLeft;
    private bool onGround;

    private float accel => speed / acceleration;
    [HideInInspector] public Vector3 velocity { get => rb.velocity; set => rb.velocity = value; }
    [HideInInspector] public Vector3 position => transform.position;
    [HideInInspector] public Vector3 centerPos => transform.position + collider.center;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        GameplayInputReader.Get(ref input);
        audioC = GetComponent<AudioCaller>();
        health = GetComponent<Health>();
        shooter = GetComponent<PlayerShooter>();
        anim = GetComponentInChildren<PlayerAnimator>();
        collider = GetComponent<CapsuleCollider>();
        colliderFoot = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        MovementDirection();
        onGround = OnGround();

        nextVelocity = velocity;

        if (dodgeTimeLeft > 0)
        {
            dodgeTimeLeft -= Time.deltaTime;
            if (dodgeTimeLeft <= 0)
            {
                dodgeTimeLeft = 0;
                health.damagable = true;
            }

            DodgeMovement();
        }
        else
        {
            BasicMovement();
            AimBasedRotation();

            if (input.jump.WasPressedThisFrame() && onGround) anim.Jump();
            if (jumping)
            {
                nextVelocity = new(nextVelocity.x, jumpSpeed, nextVelocity.z);
                jumping = false;
            }

            if (input.sprint.WasPressedThisFrame() && input.movementVector2 != Vector2.zero) BeginDodge();
        }


        if (transform.position.y < deathBarrier)
        {
            transform.position = lastGroundedPosition;
            rb.Move(lastGroundedPosition, rb.rotation);
            rb.velocity = new(rb.velocity.x, rb.velocity.y.Min(-9.81f), rb.velocity.z);
            GetComponent<Health>().Damage(25, Health.DamageType.Generic, this, "BottomlessPit");
        }

        anim.p_inAir = !onGround;
        if (onGround && dodgesLeft != amountOfAirDodges) dodgesLeft = amountOfAirDodges;

        rb.velocity = nextVelocity;
    }

    private void MovementDirection()
    {
        movementDirection = new Vector3(input.movementVector2.x, 0, input.movementVector2.y);
        movementDirection = movementDirection.Direction().Rotate(cameraTransform.eulerAngles.y, Direction.up);
    }

    private void BasicMovement()
    {
        Direction direction = (Direction)nextVelocity * Direction.XZ;

        if (movementDirection.magnitude > 0)
        {
            direction += (Direction)movementDirection * accel;
        }
        else if (movementDirection.magnitude == 0) direction -= direction.normalized * accel;
        if (direction.magnitude > speed * movementDirection.magnitude) direction = direction.normalized * speed * movementDirection.magnitude;
        /*
        #region NonSidewaysStuck Section

        bool sideHit = Physics.CapsuleCast(
            point1: transform.position + collider.center + transform.up * (collider.height / 2 - collider.radius),
            point2: transform.position + collider.center - transform.up * (collider.height / 2 - collider.radius),
            radius: collider.radius,
            direction: direction.normalized,
            hitInfo: out RaycastHit sideHitInfo,
            maxDistance: direction.magnitude * Time.deltaTime * 2,
            layerMask: groundLayers
            );
        d_hit = sideHit;

        if (sideHit)
        {
            d_Distance = sideHitInfo.distance;
            direction = direction.normalized * sideHitInfo.distance;
        }

        #endregion NonSidewaysStuck Section
         */

        direction.y = (onGround && nextVelocity.y < 0) ? 0 : nextVelocity.y;

        nextVelocity = direction;

    }

    public bool d_hit;
    public float d_Distance;

    private void DodgeMovement() => nextVelocity = dodgeDirection * dodgeSpeed;

    private void MovementBasedRotation()
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void AimBasedRotation()
    {
        Quaternion toRotation = Quaternion.LookRotation(shooter.aimDirection * Direction.XZ, transform.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private void BeginDodge()
    {
        if (!OnGround() && dodgesLeft < 1) return;
        dodgeTimeLeft = dodgeTime;
        dodgeDirection = movementDirection;
        health.damagable = false;
        anim.Dodge();
        dodgesLeft--;
    }

    public void StorePosition() => lastGroundedPosition = transform.position;

    private bool jumping;
    public void JumpCallback()
    {
        Debug.Log("Jump");
        audioC.PlaySound("Jump");
        jumping = true;
    }

    public bool OnGround()
    {

        bool hit = Physics.SphereCast(
            origin: transform.position + colliderFoot.center + Vector3.up * 0.001f,
            radius: colliderFoot.radius,
            direction: Vector3.down,
            hitInfo: out RaycastHit result,
            maxDistance: 0.005f,
            layerMask: groundLayers
            );

        return hit;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isEditor) Gizmos.DrawSphere(centerPos - Vector3.up * (collider.height / 2 - collider.radius + 0.01f), collider.radius);
    }

}