using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericAnimCallback : MonoBehaviour
{
    public UnityEvent[] events;

    public void ActivateEvent(int I) => events[I]?.Invoke();

}
