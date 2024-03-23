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


    void Start()
    {
        input = GameplayInputReader.Get();
        health = GetComponent<Health>();

    }

    void Update()
    {
        if (input.Heal.WasPressedThisFrame()) Heal();
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
    }

    void Heal()
    {
        if (health.currentHealth >= health.maxHealth) return;

        if(currentRice > 1 && currentFish > 0 && currentSeaweed > 0)
        {
            health.Heal(20);
            currentRice--;
            currentFish -= 2;
            currentSeaweed--;
        }
    }
}
