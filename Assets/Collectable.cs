using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Collectable : AbstractCollectable
{

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        isCollectable = true;
        interactText = "Grab";
    }

    // Update is called once per frame
    protected void Update()
    {
        if(beingManipulated)
        {
            int i = 0;
            if(Vector3.Distance(transform.localPosition, destinationPosition) > GameManager.I.distanceToDestination)
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

            if(Vector3.Distance(transform.localScale, destinationScale) > 0.01)
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

    public override void Collect(Entity entity)
    {
        beingManipulated = true;
        isCollectable = false;
        this.gameObject.tag = "Untagged";
        col.enabled = false;
        HandDestination(entity.hand);
    }

    public override void Interact(Entity entity)
    {
        Collect(entity);
    }

    public override void Use(Entity entity)
    {

    }

    public override void Equip(Entity entity)
    {

    }
    public override void UnEquip(Entity entity)
    {

    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    public override void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale)
    {
        destinationPosition = dPosition;
        destinationRotation = dRotation;
        destinationScale = dScale;
        beingManipulated = true;
    }

    public override void GiveDestination(Transform parent, Vector3 scale)
    {
        destinationPosition = Vector3.zero;
        destinationRotation = Vector3.zero;
        destinationScale = scale;
        beingManipulated = true;
        transform.parent = parent;
    }

    public override void GiveDestination(Transform parent)
    {
        destinationPosition = Vector3.zero;
        destinationRotation = Vector3.zero;
        //destinationScale = parent.localScale;
        beingManipulated = true;
        transform.parent = parent;
    }


    public override void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale, Transform parent)
    {
        destinationPosition = dPosition;
        destinationRotation = dRotation;
        destinationScale = dScale;
        beingManipulated = true;
        transform.parent = parent;
    }

    public override void HandDestination(Transform hand)
    {
        GiveDestination(handPosition, handRotation, handScale, hand);
    }
}
