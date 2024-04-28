using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Highlighter : MonoBehaviour
{
    public GameObject start;
    public GameObject credits;
    public GameObject howtoplay;
    public GameObject settings;
    public GameObject exit;
    public Transform transformed;

    public void SelectedStart()
    {
        transformed.LeanMoveLocal(new Vector2(960,-80), 0.1f);
        Debug.Log(start.name + " was selected");
    }

    public void SelectedCredits()
    {
        transformed.LeanMoveLocal(new Vector2(960,-158.4f), 0.1f);
        Debug.Log(credits.name + " was selected");
    }

    public void SelectedSettings()
    {
        transformed.LeanMoveLocal(new Vector2(960,-236.8f), 0.1f);
        Debug.Log(howtoplay.name + " was selected");
    }

    public void SelectedHow()
    {
        transformed.LeanMoveLocal(new Vector2(960,-315.2f), 0.1f);
        Debug.Log(settings.name + " was selected");
    }

    public void SelectedExit()
    {
        transformed.LeanMoveLocal(new Vector2(960,-393.6f), 0.1f);
        Debug.Log(exit.name + " was selected");
    }
}
