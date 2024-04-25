using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VHSTape : Collectable
{
    VideoClip vcStatic;
    VideoClip vcGrandpa;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //handScale = Vector3.one * 150;
        handRotation = new Vector3 (-30, 40, 0);
        interactText = "Pickup";
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

    public override void Use(Entity entity, bool isDown)
    {

    }

    public void PlayTape()
    {

    }
}
