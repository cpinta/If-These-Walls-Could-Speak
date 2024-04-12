using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : Interactable
{
    [SerializeField] public Transform location;
    public bool usedToHide = false;
    public bool syncRotation = false;


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
        usedToHide = true;
        col.enabled = false;
        entity.Hide(this);
    }

    public void UnHide()
    {
        usedToHide = false;
        col.enabled = true;
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }
}
