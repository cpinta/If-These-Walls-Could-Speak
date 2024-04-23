using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : Interactable
{
    VHSTape tape;
    Animator animator;
    [SerializeField] Transform tapeLocation;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Entity entity)
    {
        if (tape == null)                            //if there is no plate on the table
        {
            if (entity.currentCollectable != null)
            {
                if (entity.currentCollectable is VHSTape)     //if the player is currently holding a plate
                {
                    VHSTape playersTape = entity.currentCollectable as VHSTape;
                    tape = (VHSTape)entity.TakeCurrentItem();

                    tape.GiveDestination(tapeLocation);
                    animator.SetBool("Open", true);
                }
                else
                {
                    return;
                }
            }
        }
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {
        if (isHovering)
        {
            if (entity.HasCollectableType(typeof(VHSTape)))
            {
                interactText = "Insert Tape";
            }
            else
            {
                interactText = "";
            }
        }
    }

    public override void ResetGame()
    {
        animator.SetBool("Open", false);
        if(tape != null)
        {
            Destroy(tape);
            tape = null;
        }
    }
}
