using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnterZone : MonoBehaviour
{
    public UnityEvent EnterEvent;
    public bool OneTime = true;
    bool wasDone;

    private void OnTriggerEnter(Collider other)
    {
        if (OneTime && wasDone) return;
        if (other.GetComponent<PlayerMove>())
        {
            EnterEvent?.Invoke();
            wasDone = true;
            
        }
    }
}
