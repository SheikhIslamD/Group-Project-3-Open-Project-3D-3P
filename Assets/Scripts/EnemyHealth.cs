using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   [SerializeField] float healthPoints = 100f;
   public void TakeDamage (float damage)
   {
        healthPoints -= damage;
        if (healthPoints<=0)
        {
            Destroy(gameObject);
        }
   }
}
