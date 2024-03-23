using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 6.0f;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float gravity = 20.0F;
    [SerializeField] float rotateSpeed = 3.0f;
    private Vector3 moveDirection = Vector3.zero;
    GameplayInputReader input;

    void Start()
    {
        input = GameplayInputReader.Get();
    }
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = input.movementVector2;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}