using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Highlighter : MonoBehaviour
{
    new Transform transform;
    public Transform[] buttons;
    public float xPos = 1426.5f;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        SelectItem(0);
    }

    public void SelectItem(int id)
    {
        transform.LeanMove(new Vector2(xPos, buttons[id].position.y), 0.1f);
    }
}
