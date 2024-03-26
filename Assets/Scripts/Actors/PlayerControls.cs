using UnityEngine;
using Vector3Helper;

public class PlayerControls : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 720.0f;
    public float jumpSpeed = 5.0f;

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    Transform cameraTransform;
    GameplayInputReader input;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        cameraTransform = Camera.main.transform;
        GameplayInputReader.Get(ref input);
    }

    void Update()
    {
        Vector2 movementInput = input.movementVector2;

        Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();


        //Alters the Direction by camera.
        movementDirection = new Direction(movementDirection).Rotate(cameraTransform.eulerAngles.y, Direction.up);


        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}