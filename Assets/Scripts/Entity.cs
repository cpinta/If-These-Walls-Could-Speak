using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool moveCamera = false;
    public bool isHiding = false;
    protected HidingSpot hidingSpot;
    protected HidingState hidingState;
    protected Vector3 preHidePosition;
    public Transform hand;
    protected bool canMoveBody = true;
    protected bool canMoveCamera = true;
    public bool canInteract = true;
    [SerializeField] protected float hideLerp = 6f;
    protected bool customHide = false;
    protected float viewRadius = 10;
    protected float viewAngle = 90;
    protected int targetMask = 0;
    protected int obstructionMask = 0;
    protected bool canSeeTarget = false;
    public List<Collectable> collectables = new List<Collectable>();
    public Collectable currentCollectable;

    void Update()
    {
        if(moveCamera)
        {
            if (!customHide)
            {
                if(hidingState == HidingState.Entering)
                {
                    if (Vector3.Distance(transform.position, hidingSpot.location.position) > GM.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, hidingSpot.location.position, hideLerp * Time.deltaTime);
                        transform.rotation = Quaternion.Lerp(transform.rotation, hidingSpot.location.rotation, hideLerp * Time.deltaTime);
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, hidingSpot.location.position) > GM.I.distanceToDestination)
                    {
                        transform.position = Vector3.Lerp(transform.position, preHidePosition, hideLerp);
                    }
                }
            }
        }
    }

    public virtual void Hide(HidingSpot spot, bool hiding)
    {
        if(canInteract && canMoveBody && canMoveCamera)
        {
            Debug.Log(name + " is hiding");
            isHiding = hiding;
            moveCamera = true;
            hidingSpot = spot;
            hidingState = HidingState.Entering;
            preHidePosition = transform.position;
            canInteract = !hiding;
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
        moveCamera = false;
        canMoveBody = true;
        canInteract = true;
    }

    public virtual void StartUnHide()
    {
        hidingState = HidingState.Exiting;
    }

    public virtual void Collect(Collectable collectable)
    {
        bool equip = collectable.Collect(this);
        if(equip)
        {
            collectables.Add(collectable);
            Equip(collectable);
        }
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

    public virtual bool HasCollectable(Collectable collectable)
    {
        if(collectables.Contains(collectable)) return true;
        return false;
    }

    public bool HasCollectableType(Type type)
    {
        return collectables.Any(collectable => collectable.GetType() == type);
    }
    
    public Collectable GetCollectableType(Type type)
    {
        for(int i = 0; i < collectables.Count; i++)
        {
            if (collectables[i].GetType() == type)
            {
                return collectables[i];
            }
        }
        return null;
    }

    public bool RemoveItemOfType(Collectable collectable)
    {
        Type type = collectable.GetType();
        for (int i = 0; i < collectables.Count; i++)
        {
            if (collectables[i].GetType() == type)
            {
                RemoveItemAt(i);
                return true;
            }
        }
        return false;
    }

    void RemoveItemAt(int i)
    {
        if (currentCollectable == collectables[i])
        {
            currentCollectable = null;
        }
        Destroy(collectables[i]);
        collectables.RemoveAt(i);
    }

    public void RemoveCurrentItem()
    {
        if (currentCollectable != null)
        {
            collectables.Remove(currentCollectable);
            Destroy(currentCollectable.gameObject);
            currentCollectable = null;
        }
    }

    //takes the current item from the entity and returns it in the return statement
    public Collectable TakeCurrentItem()
    {
        if (currentCollectable != null)
        {
            Collectable collectable = currentCollectable;
            collectables.Remove(currentCollectable);
            currentCollectable = null;
            collectable.transform.parent = null;
            return collectable;
        }
        return null;
    }

    public virtual void Hit()
    {

    }

    public virtual void ResetGame()
    {

    }
}
