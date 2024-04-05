using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour, I_Interactable
{
    public string interactText = "Interact";
    public Collider col;


    protected void Start()
    {
        gameObject.tag = "Interactable";
        if(col == null)
        {
            col = GetComponent<Collider>();
        }
    }

    public abstract void Interact(Entity entity);
}