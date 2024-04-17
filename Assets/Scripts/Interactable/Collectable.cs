using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : Interactable
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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isCollectable = true;
        interactText = "Grab";
    }

    // Update is called once per frame
    protected void Update()
    {
        if (beingManipulated)
        {
            int i = 0;
            if (Vector3.Distance(transform.localPosition, destinationPosition) > GameManager.I.distanceToDestination)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, destinationPosition, Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
            }
            float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(destinationRotation));
            if (angle > 0)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(destinationRotation), Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
            }

            if (Vector3.Distance(transform.localScale, destinationScale) > 0.01)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, destinationScale, Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
            }

            if (i == 3)
            {
                beingManipulated = false;
            }
        }
    }

    public override void Interact(Entity entity)
    {
        if(isCollectable)
        {
            Collect(entity);
            return;
        }
    }

    public virtual void Collect(Entity entity)
    {
        beingManipulated = true;
        isCollectable = false;
        this.gameObject.tag = "Untagged";
        collider.enabled = false;
        HandDestination(entity.hand);
    }

    public abstract void Use(Entity entity);

    public abstract void Equip(Entity entity);
    public abstract void UnEquip(Entity entity);

    public void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale)
    {
        destinationPosition = dPosition;
        destinationRotation = dRotation;
        destinationScale = dScale;
        beingManipulated = true;
    }

    public void GiveDestination(Transform parent, Vector3 scale)
    {
        destinationPosition = Vector3.zero;
        destinationRotation = Vector3.zero;
        destinationScale = scale;
        beingManipulated = true;
        transform.parent = parent;
    }

    public void GiveDestination(Transform parent)
    {
        destinationPosition = Vector3.zero;
        destinationRotation = Vector3.zero;
        //destinationScale = parent.localScale;
        beingManipulated = true;
        transform.parent = parent;
    }


    public void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale, Transform parent)
    {
        destinationPosition = dPosition;
        destinationRotation = dRotation;
        destinationScale = dScale;
        beingManipulated = true;
        transform.parent = parent;
    }

    public void HandDestination(Transform hand)
    {
        GiveDestination(handPosition, handRotation, handScale, hand);
    }
}