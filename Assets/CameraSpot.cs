using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpot : HidingSpot
{
    protected bool isCursorLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        lockCamera = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Interact(Entity entity)
    {
        Hide(entity);
    }

    public override void Hide(Entity entity)
    {
        Debug.Log("Camera snapped to: " + name);
        usedToHide = true;
        col.enabled = false;
        if(isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        entity.Hide(this, false);
    }

    public override void UnHide()
    {
        if (!isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        usedToHide = false;
        col.enabled = true;
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }
}
