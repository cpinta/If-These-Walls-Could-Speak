using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum HidingState
{
    Entering = 0,
    In = 2,
    Exiting = 1,
}

public abstract class Entity : MonoBehaviour, IEntity
{
    public string Name;
    public bool isHiding = false;
    protected HidingSpot hidingSpot;
    protected HidingState hidingState;
    protected Vector3 preHidePosition;
    protected bool canMoveBody = true;
    protected bool canMoveCamera = true;
    protected bool canInteract = true;
    [SerializeField] protected float hideLerp = 6f;
    protected bool customHide = false;

    void Update()
    {
        if(isHiding)
        {
            if (!customHide)
            {
                if(hidingState == HidingState.Entering)
                {
                    if (Vector3.Distance(transform.position, hidingSpot.location.position) > GameManager.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, hidingSpot.location.position, hideLerp * Time.deltaTime);
                        transform.rotation = Quaternion.Lerp(transform.rotation, hidingSpot.location.rotation, hideLerp * Time.deltaTime);
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, hidingSpot.location.position) > GameManager.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, preHidePosition, hideLerp);
                        //transform.rotation = Quaternion.Lerp(transform.rotation, hidingSpot.rotation, hideLerp);
                    }
                }
            }
        }
    }

    public virtual void Hide(HidingSpot spot)
    {
        if(canInteract && canMoveBody && canMoveCamera)
        {
            Debug.Log(name + " is hiding");
            isHiding = true;
            hidingSpot = spot;
            hidingState = HidingState.Entering;
            preHidePosition = transform.position;
            canInteract = false;
            canMoveBody = false;
            if(spot.syncRotation)
            {
                canMoveCamera = false;
            }
        }
    }

    public virtual void UnHide()
    {
        hidingSpot.UnHide();
        hidingSpot = null;

        isHiding = false;
        canMoveBody = true;
        canInteract = true;
    }
}
