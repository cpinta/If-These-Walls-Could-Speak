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
        if(entity.currentCollectable != null)
        {
            if (entity.currentCollectable is Plate)
            {
                currentPlate = (Plate)entity.TakeCurrentItem();
                currentPlate.GiveDestination(transform);
            }
            else
            {
                return;
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