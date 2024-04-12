using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(beingCollected)
        {
            int i = 0;
            if(Vector3.Distance(transform.localPosition, handOffset) > GameManager.I.distanceToDestination)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, handOffset, Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
            }
            float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(handRotation));
            if (angle > 0)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(handRotation), Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
            }

            if(Vector3.Distance(transform.localScale, inHandScale) > 0.01)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, inHandScale, Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
            }

            if (i == 3)
            {
                beingCollected = false;
            }
        }
    }

    public override void Collect(Entity entity)
    {
        beingCollected = true;
        isCollectable = false;
        this.gameObject.tag = "Untagged";
        col.enabled = false;
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
}
