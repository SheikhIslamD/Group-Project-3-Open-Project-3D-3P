using System.Collections;
using UnityEngine;


public class DamageFlash : MonoBehaviour
{
    public new Renderer renderer;
    private Material mat;
    private float damageRed;

    private void Awake() => mat = renderer.material;

    private IEnumerator DamageEnum()
    {
        damageRed = 1;
        mat.SetFloat("DamageRed", damageRed);
        yield return null;
        while (damageRed > 0)
        {
            damageRed -= Time.deltaTime;
            mat.SetFloat("DamageRed", damageRed);
            yield return null;
        }
    }


    public void OnHealthChange(Health.Interaction args)
    {
        if (args.isDamage && !args.interrupted)
        {
            StartCoroutine(DamageEnum());
        }
    }
}
