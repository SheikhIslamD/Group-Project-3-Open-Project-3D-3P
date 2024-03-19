using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum DamageType { Generic, Melee, Projectile }
    
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;
    
    
    public void Damage(int amount, DamageType type)
    {
         currentHealth -= amount;
    
         gameObject.SendMessage("OnDamage");
    
         if (currentHealth<=0)
         {
             HealthDeplete();
         }
    }
    
    void HealthDeplete()
    {
        Destroy(gameObject);
        gameObject.SendMessage("OnHealthDeplete");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth) currentHealth = maxHealth;
        
        gameObject.SendMessage("OnHeal");

    }
}
