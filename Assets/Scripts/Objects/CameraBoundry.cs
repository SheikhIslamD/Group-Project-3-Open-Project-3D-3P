using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraBoundry : MonoBehaviour
{
    [HideInInspector] public BoxCollider boundry;

    [HideInInspector] public Vector3 cameraOffset;
    [HideInInspector] public Vector3 cameraRotation;
    [HideInInspector] public Vector3 backOffset;
    [HideInInspector] public Vector3 backRotation;
    public bool backFromPlayer;
    public float cameraFOV = 60f;

    Transform cameraTransform;
    Transform backTransform;
    Transform targetTransform;

    private new CameraMovement camera;
    private void Awake()
    {
        camera = CameraMovement.instance;
        BoxCollider boundry = GetComponent<BoxCollider>();

        cameraTransform = transform.Find("CameraT");
        backTransform = transform.Find("BackT");
        targetTransform = new GameObject("Target").transform;
        targetTransform.parent = transform;

        cameraOffset = cameraTransform.position - transform.position;
        cameraRotation = cameraTransform.eulerAngles;
        backOffset = backTransform.position - transform.position;
        backRotation = backTransform.eulerAngles;

        Destroy(cameraTransform.gameObject); cameraTransform = null;
        Destroy(backTransform.gameObject); backTransform = null;
}

public void ActivateBoundry()
    {
        camera.BeginSegmentTransition(this);
    }

    public Vector3 GetTargetPosition(Vector3 playerPos)
    {
        targetTransform.position = playerPos;
        targetTransform.localPosition = new Vector3(
            Mathf.Clamp(targetTransform.localPosition.x,
                boundry.center.x - ((boundry.size.x / 2) - 0.5f),
                boundry.center.x + ((boundry.size.x / 2) - 0.5f)
                ),
            Mathf.Clamp(targetTransform.localPosition.y,
                boundry.center.y - ((boundry.size.y / 2) - 0.5f),
                boundry.center.y + ((boundry.size.y / 2) - 0.5f)
                ),
            Mathf.Clamp(targetTransform.localPosition.z,
                boundry.center.z - ((boundry.size.z / 2) - 0.5f), 
                boundry.center.z + ((boundry.size.z / 2) - 0.5f)
                )
            );
        return targetTransform.position + cameraOffset;
    }
}
