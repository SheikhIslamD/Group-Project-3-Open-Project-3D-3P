using System.Collections;
using UnityEngine;


public class BottomlessPit : MonoBehaviour
{
    Vector3 storedPosition;
    Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerMove>() != null)
        {
            storedPosition = other.transform.position;
            playerTransform = other.transform;
        }
    }

    public void Return()
    {
        playerTransform.GetComponent<CharacterController>().Move(storedPosition - playerTransform.transform.position);
        playerTransform.GetComponent<Health>().Damage(25, Health.DamageType.Generic, this, "BottomlessPit");
    }
}
