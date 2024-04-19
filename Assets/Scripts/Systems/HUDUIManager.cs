using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUIManager : Singleton<HUDUIManager>
{
    public Slider health;
    public Slider bossHealth;
    public TextMeshProUGUI riceText;
    public TextMeshProUGUI fishText;
    public TextMeshProUGUI seaweedText;

    [SerializeField] private RectTransform reticle;
    [SerializeField] private float outOfIngredientAlpha = 0.3f;
    private float initialScale;

    //Data
    private CanvasGroup riceGroup;
    private CanvasGroup fishGroup;
    private CanvasGroup seaweedGroup;


    private void Awake()
    {
        initialScale = reticle.localScale.x;
        riceGroup = riceText.GetComponentInParent<CanvasGroup>();
        fishGroup = fishText.GetComponentInParent<CanvasGroup>();
        seaweedGroup = seaweedText.GetComponentInParent<CanvasGroup>();
        riceGroup.alpha = outOfIngredientAlpha;
        fishGroup.alpha = outOfIngredientAlpha;
        seaweedGroup.alpha = outOfIngredientAlpha;
    }


    public void SetReticlePos(Vector2 pos, float scale)
    {
        reticle.position = pos;
        reticle.localScale = Vector3.one * 4 * initialScale / scale;
    }

    public void UpdateHealth(int hp) => health.value = hp;

    public void UpdateBossHealth(int hp) => bossHealth.value = hp;

    public void ActivateBossSection() => bossHealth.transform.parent.parent.gameObject.SetActive(true);

    public void UpdateIngredients(int rice, int fish, int seaweed)
    {
        riceText.text = "x" + rice;
        fishText.text = "x" + fish;
        seaweedText.text = "x" + seaweed;
        riceGroup.alpha = (rice > 0) ? 1 : outOfIngredientAlpha;
        fishGroup.alpha = (fish > 0) ? 1 : outOfIngredientAlpha;
        seaweedGroup.alpha = (seaweed > 0) ? 1 : outOfIngredientAlpha;


    }
}
