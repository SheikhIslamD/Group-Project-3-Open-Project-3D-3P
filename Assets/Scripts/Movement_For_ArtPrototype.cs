using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_For_ArtPrototype : MonoBehaviour
{


    #region "Variables"
    public Rigidbody Rigid;
    public float MouseSensitivity;
    public float MoveSpeed;



    public Vector3 gravity = Physics.gravity; // Use Unity's built-in gravity
    public Camera playerCamera; // Reference to the player's camera
    #endregion

    void Update()
    {
        
        
            float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

            // Rotate the player around the y-axis based on mouse input
            Rigid.MoveRotation(Rigid.rotation * Quaternion.Euler(new Vector3(0, mouseX, 0)));

            // Calculate the new rotation for looking vertically
            Quaternion camRotation = playerCamera.transform.rotation * Quaternion.Euler(-mouseY, 0, 0);

            // Apply the new rotation to the camera
            playerCamera.transform.rotation = camRotation;

            // Move the player based on input
            Vector3 movement = transform.forward * Input.GetAxis("Vertical") * MoveSpeed + transform.right * Input.GetAxis("Horizontal") * MoveSpeed;
            Rigid.MovePosition(transform.position + movement);


        
    }
}