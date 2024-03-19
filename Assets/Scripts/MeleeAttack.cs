using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private int damage = 20;
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
            Health EnemyH = other.transform.GetComponent<Health>();

            if (EnemyH == null)
            {
                Debug.Log("Not on Enemy");

                return;
            }
            EnemyH.Damage(damage, Health.DamageType.Melee);
            Destroy(gameObject);
        }
    }
}
