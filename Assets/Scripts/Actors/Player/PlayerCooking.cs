using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Health))]
public class PlayerCooking : MonoBehaviour
{

    //Connections
    GameplayInputReader input;
    Health health;

    //Data
    int currentRice;
    int currentFish;
    int currentSeaweed;

    AudioCaller audioC;
    PlayerAnimator anim;
    PlayerMove move;
    PlayerMelee melee;
    PlayerShooter shooter;

    bool canCook = true;


    void Start()
    {
        input = GameplayInputReader.instance;
        health = GetComponent<Health>();
        audioC = GetComponent<AudioCaller>();
        anim = GetComponentInChildren<PlayerAnimator>();
        move = GetComponent<PlayerMove>();
        melee = GetComponent<PlayerMelee>();
        shooter = GetComponent<PlayerShooter>();
    }

    void Update()
    {
        if (input.heal.WasPressedThisFrame()) Heal();
    }

    public void AddIngredient(int ID)
    {
        switch (ID)
        {
            case 0: currentRice++; break;
            case 1: currentFish++; break;
            case 2: currentSeaweed++; break;
            default: Debug.LogError("Invalid Ingredient Type"); break;
        }
        HUDUIManager.i.UpdateIngredients(currentRice, currentFish, currentSeaweed);
    }

    void Heal()
    {

        if (health.GetCurrentHealth() >= health.GetMaxHealth()) return;
        if (!canCook) return;

        if(currentRice > 1 && currentFish > 0 && currentSeaweed > 0)
        {
            currentRice -= 2;
            currentFish -= 1;
            currentSeaweed -= 1;
            audioC.PlaySound("Cook");
            anim.Cook();
            move.enabled = false;
            melee.enabled = false;
            shooter.enabled = false;
            canCook = false;
        }
        HUDUIManager.i.UpdateIngredients(currentRice, currentFish, currentSeaweed);
    }
    public void HealFinishCallback()
    {
        health.Heal(20, this);
        move.enabled = true;
        melee.enabled = true;
        shooter.enabled = true;
        canCook = true;
    }



    void OnHealthUpdate()
    {
        HUDUIManager.i.UpdateHealth(health.GetCurrentHealth());
    }
}
