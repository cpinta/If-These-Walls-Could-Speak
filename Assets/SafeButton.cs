using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SafeButton : Clickable
{
    Animator animator;

    public UnityEvent buttonPressed;

    // Start is called before the first frame update
    void Start()
    {
        interactText = "Confirm";
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Pressed()
    {
        buttonPressed.Invoke();
    }

    public override void Interact(Entity entity)
    {
        animator.SetTrigger("Press");
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    public override void ResetGame()
    {

    }
}
