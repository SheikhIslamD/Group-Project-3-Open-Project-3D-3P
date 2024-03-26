using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerShooter))]
public class PlayerMelee : MonoBehaviour
{
    GameplayInputReader input;
    PlayerShooter shooter;
    float slashDistance;
    [SerializeField] Transform slashTransform;
    [SerializeField] Collider slashCollider;


    private void Start()
    {
        input = GameplayInputReader.Get();
        shooter = GetComponent<PlayerShooter>();
        slashDistance = slashTransform.localPosition.z;
    }

    void Update()
    {
        slashTransform.localPosition = slashDistance * (Vector2)shooter.aimDirection;

        if (input.melee.WasPressedThisFrame())
        {

        }
        else
        {

        }





    }

}
