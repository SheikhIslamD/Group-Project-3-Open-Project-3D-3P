using UnityEngine;
using UnityEngine.Events;

public class MenuTransition : MonoBehaviour
{
    public Transform box;
    public Transform titletext;
    public CanvasGroup background;
    public bool reverse;
    public UnityEvent AppearEvent;
    public UnityEvent ReturnEvent;

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2((!reverse) ? -Screen.height : Screen.height, 0);
        box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().setOnComplete(OnCompleteAppear);

        titletext.localPosition = new Vector2(0, (!reverse) ? -Screen.height : Screen.height);
        titletext.LeanMoveLocalY(0, 0.5f).setEaseOutExpo();
    }

    public void Return()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalX((!reverse) ? -Screen.height : Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnCompleteReturn);
    }

    private void OnCompleteAppear()
    {
        AppearEvent?.Invoke();
    }
    private void OnCompleteReturn()
    {
        ReturnEvent?.Invoke();
    }
}
