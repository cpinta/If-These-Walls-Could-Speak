using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCollectable : Interactable
{
    public bool isCollectable;
    public bool beingCollected = false;
    public float collectionLerp = 10;

    public Vector3 handRotation = Vector3.zero;
    public Vector3 handOffset = Vector3.zero;
    public Vector3 inHandScale = Vector3.one;
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

    public abstract void Equip(Entity entity);
    public abstract void UnEquip(Entity entity);
}