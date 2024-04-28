using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Interactable
{
    public bool beingManipulated = false;
    public float collectionLerp = 10;

    public Vector3 handRotation = Vector3.zero;
    public Vector3 handPosition = Vector3.zero;
    public Vector3 handScale = Vector3.one;

    protected Vector3 destinationRotation = Vector3.zero;
    protected Vector3 destinationPosition = Vector3.zero;
    protected Vector3 destinationScale = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        interactText = "Pick up";
    }

    // Update is called once per frame
    void Update()
    {
        if (beingManipulated)
        {
            int i = 0;
            if (Vector3.Distance(transform.localPosition, destinationPosition) > GM.I.distanceToDestination)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, destinationPosition, Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
                transform.localPosition = destinationPosition;
            }
            float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(destinationRotation));
            if (angle > 0)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(destinationRotation), Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
                transform.localRotation = transform.localRotation;
            }

            if (Vector3.Distance(transform.localScale, destinationScale) > 0.01)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, destinationScale, Time.deltaTime * collectionLerp);
            }
            else
            {
                i++;
                transform.localScale = destinationScale;
            }

            if (i == 3)
            {
                beingManipulated = false;
                gameObject.SetActive(false);
            }
        }
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    public override void Interact(Entity entity)
    {
        if(entity is PlayerController)
        {
            PlayerController player = entity as PlayerController;
            player.FlashLightPickup();
            HandDestination(player.transform);
        }
    }

    public override void ResetGame()
    {

    }

    public void HandDestination(Transform hand)
    {
        GiveDestination(new Vector3(-0.25f, 0, 0.25f), new Vector3(90, 0, 0), Vector3.zero, hand);
    }

    public void GiveDestination(Vector3 dPosition, Vector3 dRotation, Vector3 dScale, Transform parent)
    {
        destinationPosition = dPosition;
        destinationRotation = dRotation;
        destinationScale = dScale;
        beingManipulated = true;
        transform.parent = parent;
    }
}
