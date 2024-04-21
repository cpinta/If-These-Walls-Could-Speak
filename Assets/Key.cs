using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectable
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        handRotation = new Vector3 (0, 180, 120);
        handScale = Vector3.one * 3;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }


    public override void Equip(Entity entity)
    {

    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    public override void UnEquip(Entity entity)
    {

    }

    public override void Use(Entity entity)
    {

    }

}
