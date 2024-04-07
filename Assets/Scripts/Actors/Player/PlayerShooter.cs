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
    [SerializeField] Transform handTransform;
    [SerializeField] float lineFactor;

    public Vector3 aimDirection;
    PlayerAnimator anim;
    HUDUIManager hud;

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
        hud = HUDUIManager.instance;
    }

    private void Update()
    {
        Vector3 end = handTransform.position;
        Ray cameraRay = camera.ScreenPointToRay(input.aimOutput);

        bool firstHitDidHit = Physics.Raycast(cameraRay, out RaycastHit firstHit, aimRaycastMaxDistance, aimRaycastLayerMask);

        if (firstHitDidHit) end = firstHit.point;
        else
        {
            bool secondHitDidHit = backCollider.Raycast(cameraRay, out RaycastHit secondHit, Mathf.Infinity);
            if (secondHitDidHit) end = secondHit.point;
        }

        DrawAimLine(handTransform.position, end);

        aimDirection = end - handTransform.position;

        hud.SetReticlePos(camera.WorldToScreenPoint(end));

        if (input.shoot.WasPressedThisFrame()) ShootKnife(end - transform.position);
    }

    private void DrawAimLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, Vector3.Lerp(start, end, lineFactor));
        lineRenderer.SetPosition(1, end);
    }

    private void ShootKnife(Vector3 direction)
    {
        audioC.PlaySound("Shoot");
        PoolableObject knife = pool.Pump();
        knife.Prepare_Basic(transform.position, Quaternion.FromToRotation(Vector3.up, direction).eulerAngles, Vector3.up * knifeSpeed);

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
        knife.Prepare_Basic(handTransform.position, Quaternion.FromToRotation(Vector3.up, aimDirection).eulerAngles, Vector3.up * knifeSpeed);
    }

}