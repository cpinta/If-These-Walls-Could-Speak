using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ControlState
{
    FullControl = 3,
    CantMove = 2,
    CantInteract = 1,
    NoControl = 0
}

public enum HidingStatus
{
    Entering = 0,
    Exiting = 1,
}

public abstract class Entity : MonoBehaviour, IEntity
{
    public string Name;
    public bool isHiding = false;
    protected Transform hidingSpot;
    protected HidingStatus hidingStatus;
    protected Vector3 preHidePosition;
    protected ControlState controlState;
    protected float hideLerp = 0.7f;
    protected bool customHide = false;

    void Update()
    {
        if(isHiding)
        {
            if (customHide)
            {
                if(hidingStatus == HidingStatus.Entering)
                {
                    if (Vector3.Distance(transform.position, hidingSpot.position) > GameManager.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, hidingSpot.position, hideLerp);
                        transform.rotation = Quaternion.Lerp(transform.rotation, hidingSpot.rotation, hideLerp);
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, hidingSpot.position) > GameManager.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, preHidePosition, hideLerp);
                        //transform.rotation = Quaternion.Lerp(transform.rotation, hidingSpot.rotation, hideLerp);
                    }
                }
            }
        }
    }

    public virtual void Hide(Transform spot)
    {
        if(controlState == ControlState.FullControl)
        {
            Debug.Log(name + " is hiding");
            isHiding = true;
            hidingSpot = spot;
            hidingStatus = HidingStatus.Entering;
            preHidePosition = transform.position;
        }
    }
}
