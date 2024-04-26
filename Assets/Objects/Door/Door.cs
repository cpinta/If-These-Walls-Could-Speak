using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    Animator animator;
    bool isOpen = false;
    bool initialLockState = false;
    [SerializeField] bool locked = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if(locked)
        {
            interactText = "Locked";
            interactInput = false;
        }
        initialLockState = locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Entity entity)
    {
        if (locked)
        {
            if (entity.currentCollectable is Key)
            {
                entity.RemoveCurrentItem();
                Unlock();
            }
        }
        else
        {
            ToggleOpenClose();
        }
    }

    void Unlock()
    {
        locked = false;
    }

    void ToggleOpenClose()
    {
        if(isOpen)
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

    public override void IsHovering(bool isHovering, Entity entity)
    {
        if (locked)
        {
            if (entity.currentCollectable is Key)
            {
                interactText = "Unlock";
                interactInput = true;
            }
            else
            {
                interactText = "Locked";
                interactInput = false;
            }
        }
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
