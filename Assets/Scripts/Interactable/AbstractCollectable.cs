using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCollectable : Interactable
{
    public bool isCollectable;
    public Item item;

    public void Interact()
    {
        if(isCollectable)
        {
            Collect();
            return;
        }
    }
    public abstract void Collect();
}