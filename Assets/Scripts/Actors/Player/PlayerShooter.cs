using UnityEngine;


public class PlayerShooter : MonoBehaviour
{
    private new Transform transform;
    private LineRenderer lineRenderer;
    private new Camera camera;
    private GameplayInputReader input;
    private ObjectPool pool;
    private Collider backCollider;
    private AudioCaller audioC;
    [SerializeField] private LayerMask aimRaycastLayerMask;
    [SerializeField] private float aimRaycastMaxDistance;
    [SerializeField] private float knifeSpeed;

    public Vector3 aimDirection;
    PlayerAnimator anim;


    private void Start()
    {
        input = GameplayInputReader.instance;
        transform = GetComponent<Transform>();
        camera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        backCollider = FindFirstObjectByType<CameraMovement>().backCollider.GetComponent<Collider>();
        pool = GetComponent<ObjectPool>();
        audioC = GetComponent<AudioCaller>();
        anim = GetComponentInChildren<PlayerAnimator>();
    }

    private void Update()
    {
        Vector3 end = transform.position;
        Ray cameraRay = camera.ScreenPointToRay(input.aimOutput);

        bool firstHitDidHit = Physics.Raycast(cameraRay, out RaycastHit firstHit, aimRaycastMaxDistance, aimRaycastLayerMask);

        if (firstHitDidHit) end = firstHit.point;
        else
        {
            bool secondHitDidHit = backCollider.Raycast(cameraRay, out RaycastHit secondHit, Mathf.Infinity);
            if (secondHitDidHit) end = secondHit.point;
        }

        DrawAimLine(transform.position, end);

        aimDirection = end - transform.position;

        if (input.shoot.WasPressedThisFrame()) ShootKnife();
    }

    private void DrawAimLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void ShootKnife()
    {
        audioC.PlaySound("Shoot");
        anim.Throw();

        /*
        knife.gameObject.SetActive(true);
        knife.transform.position = transform.position;
        knife.transform.eulerAngles = Vector3.zero;
        knife.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        knife.transform.GetComponent<Rigidbody>().velocity = knife.transform.up * knifeSpeed;
         */

    }

    public void KnifeCallback()
    {
        PoolableObject knife = pool.Pump();
        knife.Prepare_Basic(transform.position, Quaternion.FromToRotation(Vector3.up, aimDirection).eulerAngles, Vector3.up * knifeSpeed);
    }

}
