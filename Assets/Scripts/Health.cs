using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum DamageType { Generic, Melee, Projectile }
    //[System.Flags]
    //public enum EntityType { Player, Enemy, Boss, Object, Other }
    
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;
    //[SerializeField] public EntityType entityType;
    
    public void Damage(int amount, DamageType type)
    {
         currentHealth -= amount;
    
         gameObject.SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
    
         if (currentHealth<=0)
         {
             HealthDeplete();
         }
    }
    
    void HealthDeplete()
    {
        gameObject.SendMessage("OnHealthDeplete", SendMessageOptions.DontRequireReceiver);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth) currentHealth = maxHealth;
        
        gameObject.SendMessage("OnHeal", SendMessageOptions.DontRequireReceiver);

    }
}
