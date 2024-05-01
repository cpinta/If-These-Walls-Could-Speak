using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : Door
{
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        initialLockState = locked;
        interactText = "";
        this.gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Entity entity)
    {

    }

    public void ForceOpen()
    {
        isOpen = false;
        Open();
    }

    public void Open()
    {
        if (!isOpen)
        {
            animator.SetBool("Open", true);
            interactText = "";
        }
    }

    protected void Opened()
    {
        isOpen = true;
        interactText = "";
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {
        interactText = "";
    }

    public override void ResetGame()
    {
        if (isOpen)
        {
            Close();
            isOpen = false;
            interactText = "Open";
        }
        locked = initialLockState;
    }
}
