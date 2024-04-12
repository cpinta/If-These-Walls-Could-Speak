using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Collectable
{
    bool placed = true;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //handRotation = new Vector3(-42, -138, 0);
        //handOffset = new Vector3(0, 0.1f, 0);
        //inHandScale = new Vector3(0.7f, 0.7f, 0.7f);

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Collect(Entity entity)
    {
        base.Collect(entity);
    }

    public override void Interact(Entity entity)
    {
        base.Interact(entity);
    }

    public override void Use(Entity entity)
    {
        base.Use(entity);
    }

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
    }
    public override void UnEquip(Entity entity)
    {
        base.UnEquip(entity);
    }
}
