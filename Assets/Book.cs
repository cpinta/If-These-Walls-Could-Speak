using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Collectable
{
    Animator animator;
    Rigidbody rb;
    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        handRotation = new Vector3(-42, -138, 0);
        handOffset = new Vector3(0, 0.1f, 0);
        inHandScale = new Vector3(0.7f, 0.7f, 0.7f);

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Collect(Entity entity)
    {
        base.Collect(entity);
        rb.isKinematic = true;
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
        base.Use(entity);
        //ToggleOpenClose();
    }

    public override void Equip(Entity entity)
    {
        base.Equip(entity);
        isOpen = false;
        Open();
    }
    public override void UnEquip(Entity entity)
    {
        base.UnEquip(entity);
        isOpen = true;
        Close();
    }
}
