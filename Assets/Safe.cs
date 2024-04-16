using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : CameraSpot
{
    [SerializeField] SafeDial[] dials;
    bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        syncRotation = true;
        isCursorLocked = false;
        TurnOffDials();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Entity entity)
    {
        base.Interact(entity);
        TurnOnDials();
    }

    public override void UnHide()
    {
        base.UnHide();
        TurnOffDials();
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    void TurnOffDials()
    {
        for(int i = 0; i < dials.Length; i++)
        {
            dials[i].collider.enabled = false;
        }
    }
    void TurnOnDials()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].collider.enabled = true;
        }
    }
}