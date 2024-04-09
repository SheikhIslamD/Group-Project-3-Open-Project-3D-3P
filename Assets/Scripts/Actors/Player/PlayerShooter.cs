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
    [SerializeField] private Transform handTransform;
    [SerializeField] private float lineFactor;

    public Vector3 aimDirection;
    private PlayerAnimator anim;
    private HUDUIManager hud;

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

        bool firstHitDidHit = Physics.Raycast(cameraRay, out RaycastHit hit, aimRaycastMaxDistance, aimRaycastLayerMask);

        if (firstHitDidHit) end = hit.point;
        else
        {
            bool secondHitDidHit = backCollider.Raycast(cameraRay, out hit, Mathf.Infinity);
            if (secondHitDidHit) end = hit.point;
        }

        DrawAimLine(handTransform.position, end);

        aimDirection = end - handTransform.position;

        hud.SetReticlePos(camera.WorldToScreenPoint(end), hit.distance);

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
        knife.Prepare_Basic(handTransform.position, Quaternion.FromToRotation(Vector3.up, aimDirection).eulerAngles, Vector3.up * knifeSpeed);
    }

}