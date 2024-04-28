using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Highlighter : MonoBehaviour
{
    public Transform highlighter;
    public Transform[] buttons;

    private void Awake() => StartCoroutine(BeginLate());

    private IEnumerator BeginLate()
    {
        yield return new WaitForEndOfFrame();
        buttons[0].GetComponent<Selectable>().Select();
        highlighter.LeanMove(new Vector2(Screen.width, buttons[0].position.y), 0.1f);
    }

    public void SelectItem(int id) => highlighter.LeanMoveY(buttons[id].position.y, 0.1f);//Debug.LogFormat("Move to Item {0} at Position {1} named {2}", id, buttons[id].position.y, buttons[id].name);

}
