using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuTransition : MonoBehaviour
{
    public Transform box;
    public Transform titletext;
    public CanvasGroup background;

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(-Screen.height, 0);
        box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;

        titletext.localPosition = new Vector2(0, -Screen.height);
        titletext.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.5f;
    }

    public void Return()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalX(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete);
    }

    void OnComplete()
    {
        gameObject.SetActive(false);
    }
}
