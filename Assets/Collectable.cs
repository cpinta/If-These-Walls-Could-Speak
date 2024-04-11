using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : AbstractCollectable
{

    // Start is called before the first frame update
    void Start()
    {
        isCollectable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Collect(Entity entity)
    {

    }

    public override void Interact(Entity entity)
    {
        Collect(entity);
    }

    public override void Use(Entity entity)
    {

    }
}
