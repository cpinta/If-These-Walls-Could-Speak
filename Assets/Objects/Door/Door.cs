using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    protected Animator animator;
    protected bool isOpen = false;
    protected bool initialLockState = false;
    [SerializeField] protected bool locked = false;
    [SerializeField] protected bool initiallyOpen = false;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if(locked)
        {
            interactText = "Locked";
            interactInput = false;
        }
        initialLockState = locked;

        if(initiallyOpen)
        {
            animator.SetBool("Open", true);
            isOpen = true;
            animator.SetTrigger("InitiallyOpen");
        }

        GM.I.loadPhase1.AddListener(ResetGame);
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

    protected void Unlock()
    {
        locked = false;
        interactText = "Open";
    }

    protected void ToggleOpenClose()
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

    public void Open()
    {
        if (!isOpen)
        {
            animator.SetBool("Open", true);
            interactText = "";
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            animator.SetBool("Open", false);
            interactText = "";
        }
    }

    protected void Opened()
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
        if (initiallyOpen)
        {
            animator.SetBool("Open", true);
            isOpen = true;
            animator.SetTrigger("InitiallyOpen");
        }
        else
        {
            if (isOpen)
            {
                Close();
                isOpen = false;
                interactText = "Open";
            }
        }
        locked = initialLockState;
    }
}
