using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCollectable : Interactable
{
    public bool isCollectable;
    public bool beingManipulated = false;
    public float collectionLerp = 10;


    public Vector3 handRotation = Vector3.zero;
    public Vector3 handPosition = Vector3.zero;
    public Vector3 handScale = Vector3.one;

    protected Vector3 destinationRotation = Vector3.zero;
    protected Vector3 destinationPosition = Vector3.zero;
    protected Vector3 destinationScale = Vector3.one;
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

    public abstract void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale);
    public abstract void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale, Transform parent);
    public abstract void GiveDestination(Transform parent, Vector3 scale);
    public abstract void GiveDestination(Transform parent);
    public abstract void HandDestination(Transform hand);
}