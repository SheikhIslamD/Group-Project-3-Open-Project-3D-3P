using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public enum DamageType { Generic, Melee, Projectile }
    [System.Flags]
    public enum EntityType { Player, Enemy, Boss, Object, Other }

    public bool damagable = true;

    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;
    [SerializeField] public EntityType entityType;

    //Messages
    [SerializeField] string healthChangeMessage = "OnHealthChange";
    [SerializeField] UnityEvent<int> healthChangeEvent;

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    public class Interaction
    {
        public Interaction(int amount, DamageType type, MonoBehaviour source, Health reciever, string customIdentifier = null)
        {
            this.amount = amount;
            this.type = type;
            this.source = source;
            this.reciever = reciever;
            this.customIdentifier = customIdentifier;
            interrupted = false;
            expectedFinalAmount = -1;
        }

        public int amount;
        public int expectedFinalAmount;
        public DamageType type;
        public MonoBehaviour source;
        public Health reciever;
        public string customIdentifier;
        public bool interrupted;

        public bool isDamage => amount < 0;
        public bool isHealing => amount > 0;
        public bool depletes => expectedFinalAmount == 0 && !interrupted;
        public bool restored => expectedFinalAmount == reciever.GetMaxHealth() && !interrupted;

        public void Interrupt() => interrupted = true;
    }


    public Interaction Damage(int amount, DamageType type, MonoBehaviour source, string customIdentifier = null)
    => ChangeHealth(-amount, type, source, customIdentifier);

    public Interaction Heal(int amount, MonoBehaviour source, string customIdentifier = null)
    => ChangeHealth(amount, DamageType.Generic, source, customIdentifier);

    public Interaction ChangeHealth(int amount, DamageType type, MonoBehaviour source, string customIdentifier = null)
    {
        Interaction args = new Interaction(amount, type, source, this, customIdentifier);
        

        if (!damagable) args.interrupted = true;

        args.expectedFinalAmount = currentHealth + args.amount;
        if (args.expectedFinalAmount < 0) args.expectedFinalAmount = 0;
        if (args.expectedFinalAmount > maxHealth) args.expectedFinalAmount = maxHealth;

        SendMessage(healthChangeMessage, args, SendMessageOptions.DontRequireReceiver);

        if (args.expectedFinalAmount < 0) args.expectedFinalAmount = 0;
        if (args.expectedFinalAmount > maxHealth) args.expectedFinalAmount = maxHealth;

        if (args.interrupted) return args;

        currentHealth = args.expectedFinalAmount;

        healthChangeEvent?.Invoke(currentHealth);

        var eter = 23f;

        return args;
        //if (cleanup) args = null;
        //Note: investigate the Garbage Collection (Or lack thereof) of custom classes.    }
    }


    /* OBSOLETE

    //string onDamageMessage = "OnDamage";
    //string onDepleteMessage = "OnDeplete";
    //string onHealMessage = "OnHeal";
    //string onHealthUpdateMessage = "OnHealthUpdate";

    void SendMessage(string name, DamageArgs args) { if (name != null) SendMessage(name, args, SendMessageOptions.DontRequireReceiver); }
    public void DamageOld(int amount, DamageType type)
    {
        if (!damagable) return;

        currentHealth -= amount;

        //SendMessage(onDamageMessage);
        //SendMessage(onHealthUpdateMessage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //HealthDeplete();
        }
    }
     */
}
