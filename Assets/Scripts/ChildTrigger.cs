using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChildTrigger : MonoBehaviour
{
    public UnityEvent<Collider> TriggerEntered;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Child trigger triggered on " + other.name);
        TriggerEntered.Invoke(other);
    }
}

