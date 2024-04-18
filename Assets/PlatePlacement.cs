using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatePlacement : Interactable
{
    Plate currentPlate;


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
        if (currentPlate == null)
        {
            if (entity.currentCollectable != null)
            {
                if (entity.currentCollectable is Plate)
                {
                    Plate playersPlate = entity.currentCollectable as Plate;
                    if (playersPlate.isStack)
                    {
                        currentPlate = playersPlate.TakePlate();
                    }
                    else
                    {
                        currentPlate = (Plate)entity.TakeCurrentItem();
                    }
                    currentPlate.GiveDestination(transform);
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
                currentPlate = null;
            }
            else //if the player doesn't have any plates
            {
                entity.Collect(currentPlate);
            }
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
}