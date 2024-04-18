using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Collectable
{
    Animator animator;
    Rigidbody rb;
    bool isOpen = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        handRotation = new Vector3(-42, -138, 0);
        handPosition = new Vector3(0, 0.1f, 0);
        handScale = new Vector3(0.7f, 0.7f, 0.7f);

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override bool Collect(Entity entity)
    {
        base.Collect(entity);
        rb.isKinematic = true;
        return true;
    }

    public override void Interact(Entity entity)
    {
        base.Interact(entity);
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
            isOpen = true;
        }
    }

    void Close()
    {
        if (isOpen)
        {
            animator.SetBool("Open", false);
            isOpen = false;
        }
    }

    void Opened()
    {
        isOpen = true;
    }

    void Closed()
    {
        isOpen = false;

    }
    public override void Use(Entity entity)
    {

    }

    public override void Equip(Entity entity)
    {
        isOpen = false;
        Open();
    }
    public override void UnEquip(Entity entity)
    {
        isOpen = true;
        Close();
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }
}
