using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerShooter : MonoBehaviour
{
    new Transform transform;
    private LineRenderer lineRenderer;
    new Camera camera;
    GameplayInputReader input;
    ObjectPool pool;

    Collider backCollider;
    [SerializeField] LayerMask aimRaycastLayerMask;
    [SerializeField] float aimRaycastMaxDistance;
    [SerializeField] float knifeSpeed;

    public Vector3 aimDirection;


    void Start()
    {
        input = GameplayInputReader.instance;
        transform = GetComponent<Transform>();
        camera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        backCollider = FindFirstObjectByType<CameraMovement>().backCollider.GetComponent<Collider>();
        pool = GetComponent<ObjectPool>();
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

        aimDirection = end - transform.position;

        if (input.shoot.WasPressedThisFrame()) ShootKnife(end - transform.position);
    }


    void DrawAimLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void ShootKnife(Vector3 direction)
    {
        PoolableObject knife = pool.Pump();
        knife.gameObject.SetActive(true);
        knife.transform.position = transform.position;
        knife.transform.eulerAngles = Vector3.zero;
        knife.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        knife.transform.GetComponent<Rigidbody>().velocity = knife.transform.up * knifeSpeed;
    }

}
