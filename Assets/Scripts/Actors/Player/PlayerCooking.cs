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


    void Start()
    {
        input = GameplayInputReader.instance;
        health = GetComponent<Health>();
        audioC = GetComponent<AudioCaller>();

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
        audioC.PlaySound("Cook");
        if (health.GetCurrentHealth() >= health.GetMaxHealth()) return;

        if(currentRice > 1 && currentFish > 0 && currentSeaweed > 0)
        {
            health.Heal(20);
            currentRice -= 2;
            currentFish -= 1;
            currentSeaweed -= 1;
        }
        HUDUIManager.i.UpdateIngredients(currentRice, currentFish, currentSeaweed);
    }

    void OnHealthUpdate()
    {
        HUDUIManager.i.UpdateHealth(health.GetCurrentHealth());
    }
}
