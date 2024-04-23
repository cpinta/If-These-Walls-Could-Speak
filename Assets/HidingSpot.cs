using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : Interactable
{
    [SerializeField] public Transform location;
    public bool usedToHide = false;     //means it is currently being used as a hiding spot
    public bool syncRotation = false;
    public bool lockCamera = false;


    // Start is called before the first frame update
    protected override void Start()
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

    public virtual void Hide(Entity entity)
    {
        Debug.Log("Hiding: "+name);
        usedToHide = true;
        collider.enabled = false;
        entity.Hide(this, true);
    }

    public virtual void UnHide()
    {
        usedToHide = false;
        collider.enabled = true;
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    public override void ResetGame()
    {
        usedToHide = false;
    }
}
