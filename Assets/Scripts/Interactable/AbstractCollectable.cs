using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCollectable : Interactable
{
    public bool isCollectable;
    public Item item;

    public override void Interact(Entity entity)
    {
        if(isCollectable)
        {
            Collect(entity);
            return;
        }
    }
    public abstract void Collect(Entity entity);

    public abstract void Use(Entity entity);
}