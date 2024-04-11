using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Collectable
{
    Animator animator;
    bool isOpen = false;

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
        ToggleOpenClose();
    }

    void ToggleOpenClose()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    void Open()
    {
        if (!isOpen)
        {
            animator.SetBool("Open", true);
            interactText = "";
        }
    }

    void Close()
    {
        if (isOpen)
        {
            animator.SetBool("Open", false);
            interactText = "";
        }
    }

    void Opened()
    {
        isOpen = true;
        interactText = "Close";
    }

    void Closed()
    {
        isOpen = false;
        interactText = "Open";

    }
}
