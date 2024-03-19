using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float damage= 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHealth EnemyH = other.transform.GetComponent<EnemyHealth>();

            if (EnemyH == null)
            {
                Debug.Log("Not on Enemy");

                return;
            }
            EnemyH.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
