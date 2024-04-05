using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : Interactable
{
    [SerializeField] Transform location;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        interactText = "Hide";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Interact(Entity entity)
    {
        Hide(entity);
    }

    public void Hide(Entity entity)
    {
        Debug.Log("Hiding: "+name);
        entity.Hide(location);
    }
}
