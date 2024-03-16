using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    new Transform transform;
    private LineRenderer lineRenderer;
    new Camera camera;

    Collider backCollider;
    [SerializeField] LayerMask aimRaycastLayerMask;
    [SerializeField] float aimRaycastMaxDistance;

    void Start()
    {
        transform = GetComponent<Transform>();
        camera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        backCollider = FindFirstObjectByType<CameraMovementScript>().backCollider.GetComponent<Collider>();
    }

    void Update()
    {
        Vector3 end = transform.position;
        Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit firstHit;
        bool firstHitDidHit = Physics.Raycast(cameraRay, out firstHit, aimRaycastMaxDistance, aimRaycastLayerMask);

        if (firstHitDidHit) end = firstHit.point;
        else
        {
            RaycastHit secondHit;
            bool secondHitDidHit = backCollider.Raycast(cameraRay, out secondHit, Mathf.Infinity);
            if(secondHitDidHit) end = secondHit.point;
        }

        DrawAimLine(transform.position, end);
    }


    void DrawAimLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

}
