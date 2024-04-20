using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatePlacement : Interactable
{
    Plate currentPlate;

    public UnityEvent<PlatePlacement> platePlaced = new UnityEvent<PlatePlacement>();

    // Start is called before the first frame update
    void Start()
    {
        interactText = "Need Plate";
        currentPlate = null;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Entity entity)
    {
        if (currentPlate == null)       //if there is no plate on the table
        {
            if (entity.currentCollectable != null)
            {
                if (entity.currentCollectable is Plate)     //if the player is currently holding a plate
                {
                    Plate playersPlate = entity.currentCollectable as Plate;
                    if (playersPlate.isStack)               //if it is a stack of plates
                    {
                        currentPlate = playersPlate.TakePlate();
                    }
                    else
                    {
                        currentPlate = (Plate)entity.TakeCurrentItem();
                    }
                    currentPlate.GiveDestination(transform);
                    platePlaced.Invoke(this);
                }
                else
                {
                    return;
                }
            }
        }
        else // if theres a plate on the table
        {
            if (entity.HasCollectableType(typeof(Plate)))       //if the player already has plates
            {
                Plate playersPlate = (Plate)entity.GetCollectableType(typeof(Plate));
                playersPlate.AddPlate(currentPlate);
            }
            else //if the player doesn't have any plates
            {
                entity.Collect(currentPlate);
            }
            currentPlate = null;
        }
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {
        if(isHovering)
        {
            if (entity.HasCollectableType(typeof(Plate)))
            {
                interactText = "Place Plate";
            }
            else
            {
                interactText = "";
            }
        }
    }

    public bool HasPlate()
    {
        return currentPlate != null;
    }
}