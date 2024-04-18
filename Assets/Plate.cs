using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Plate : Collectable
{
    bool placed = true;
    public bool isStack = false;
    List<Plate> plateStack = new List<Plate>();

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override bool Collect(Entity entity)
    {
        Plate originPlate = (Plate)entity.GetCollectableType(typeof(Plate));
        if (originPlate != null)
        {
            if (isCollectable)
            {
                originPlate.AddPlate(this);
                return false;
            }
        }
        else
        {
            base.Collect(entity);
        }
        return true;
    }

    public void AddPlate(Plate plate)
    {
        plate.GiveDestination(Vector3.up * 0.03f * (plateStack.Count + 1), transform.localEulerAngles, transform.localScale, transform);
        plateStack.Add(plate);
        isStack = true;
        plate.collider.enabled = false;
    }

    public Plate TakePlate()
    {
        Plate takenPlate = plateStack[plateStack.Count - 1];
        plateStack.Remove(takenPlate);
        if(plateStack.Count == 0)
        {
            isStack = false;
        }
        return takenPlate;
    }

    public override void Interact(Entity entity)
    {
        base.Interact(entity);
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
