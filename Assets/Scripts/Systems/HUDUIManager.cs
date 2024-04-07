using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDUIManager : Singleton<HUDUIManager>
{
    public Slider health;
    public TextMeshProUGUI riceText;
    public TextMeshProUGUI fishText;
    public TextMeshProUGUI SeaweedText;

    [SerializeField] RectTransform reticle;


    public void SetReticlePos(Vector2 pos)
    {
        reticle.position = pos;
    }

    public void UpdateHealth(int hp)
    {
        health.value = hp;
    }

    public void UpdateIngredients(int rice, int fish, int seaweed)
    {
        riceText.text = "Rice : " + rice;
        fishText.text = "Fish : " + fish;
        SeaweedText.text = "Seaweed : " + seaweed;
    }
}
