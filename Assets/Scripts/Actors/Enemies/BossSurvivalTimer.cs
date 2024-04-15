using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossSurvivalTimer : MonoBehaviour
{
    //Config
    [SerializeField] bool on;
    [SerializeField] float time;
    [SerializeField] private GameObject bossHealthSlider;
    [SerializeField] UnityEvent<float> updateEvent;
    [SerializeField] UnityEvent finishEvent;


    //Data
    float timer;


    public void TURNON()
    {
        on = true;
        bossHealthSlider.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!on) return;

        if (timer < time)
        {
            timer += Time.deltaTime;
            updateEvent?.Invoke(timer/time);
        }
        else
        {
            finishEvent?.Invoke();
        }
    }
}
