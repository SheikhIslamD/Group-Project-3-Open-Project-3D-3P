using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_For_ArtPrototype : MonoBehaviour
{


    #region "Variables"
    public Rigidbody rb;
    public float MouseSensitivity;
    public float MoveSpeed;



    public Vector3 gravity = Physics.gravity; // Use Unity's built-in gravity
    public Camera playerCamera; // Reference to the player's camera
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
            
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        // Rotate the player around the y-axis based on mouse input
        rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, mouseX, 0)));

        // Calculate the new rotation for looking vertically
        Quaternion camRotation = playerCamera.transform.rotation * Quaternion.Euler(-mouseY, 0, 0);

        // Apply the new rotation to the camera
        playerCamera.transform.rotation = camRotation;

        // Move the player based on input
        Vector2 control = GameplayInputReader.i.movementVector2;
        Vector3 movement = ((transform.forward * control.y) + (transform.right * control.x)) * MoveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);


        
    }

    void OnPause()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void OnUnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

}