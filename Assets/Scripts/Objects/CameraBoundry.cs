using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraBoundry : MonoBehaviour
{
    [HideInInspector] public BoxCollider boundry;
    [HideInInspector] public new Transform transform;

    public Vector3 cameraOffset => cameraTransform.position - transform.position;
    public Vector3 cameraRotation => cameraTransform.eulerAngles;
    public Vector3 backOffset => backTransform.position - transform.position;
    public Vector3 backRotation => backTransform.eulerAngles;
    public bool backFromPlayer;

    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform backTransform;

    private new CameraMovement camera;
    private void Awake()
    {
        camera = CameraMovement.Get();
        boundry = GetComponent<BoxCollider>();
        transform = base.transform;
    }

    public void ActivateBoundry()
    {
        camera.BeginSegmentTransition(this);
    }
}
