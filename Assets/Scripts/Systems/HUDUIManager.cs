using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDUIManager : Singleton<HUDUIManager>
{
    public Slider health;
    public Slider bossHealth;
    public TextMeshProUGUI riceText;
    public TextMeshProUGUI fishText;
    public TextMeshProUGUI SeaweedText;

    [SerializeField] RectTransform reticle;
    float initialScale;

    private void Awake()
    {
        initialScale = reticle.localScale.x;
    }


    public void SetReticlePos(Vector2 pos, float scale)
    {
        reticle.position = pos;
        reticle.localScale = Vector3.one * initialScale / scale;
    }

    public void UpdateHealth(int hp)
    {
        health.value = hp;
    }

    public void UpdateBossHealth(int hp)
    {
        bossHealth.value = hp;
    }

    public void UpdateIngredients(int rice, int fish, int seaweed)
    {
        riceText.text = "Rice : " + rice;
        fishText.text = "Fish : " + fish;
        SeaweedText.text = "Seaweed : " + seaweed;
    }
}
