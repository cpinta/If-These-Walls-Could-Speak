using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    public Transform hand;
    protected bool canMoveBody = true;
    protected bool canMoveCamera = true;
    protected bool canInteract = true;
    [SerializeField] protected float hideLerp = 6f;
    protected bool customHide = false;
    protected float viewRadius = 10;
    protected float viewAngle = 90;
    protected int targetMask = 0;
    protected int obstructionMask = 0;
    protected bool canSeeTarget = false;
    protected List<Collectable> collectables = new List<Collectable>();
    protected Collectable currentCollectable;

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

    public virtual void Collect(Collectable collectable)
    {
        collectable.Collect(this);
        if(hand != null)
        {
            collectable.transform.parent = hand;
        }
        else
        {
            collectable.transform.parent = transform;
        }
        collectables.Add(collectable);
        Equip(collectable);
    }

    protected virtual Entity FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, LayerMask.GetMask("Entity"));

        if (rangeChecks.Length != 0)
        {
            Entity entity = rangeChecks[0].GetComponent<Entity>();
            if(entity.isHiding)
            {
                return null;
            }

            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    return entity;
                else
                    return null;
            }
            else
                return null;
        }
        else
        {
            return null;
        }
    }

    public virtual void Equip(Collectable collectable)
    {
        if(currentCollectable != null)
        {
            currentCollectable.gameObject.SetActive(false);
        }
        currentCollectable = collectable;
        currentCollectable.gameObject.SetActive(true);
        collectable.Equip(this);
    }
}
