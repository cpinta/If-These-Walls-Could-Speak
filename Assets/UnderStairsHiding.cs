using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class UnderStairsHiding : HidingSpot
{
    Animator animator;

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

    public override void Hide(Entity entity)
    {
        base.Hide(entity);
        animator.SetBool("Hiding", true);
    }

    public override void UnHide()
    {
        base.UnHide();
        animator.SetBool("Hiding", false);
    }
}
