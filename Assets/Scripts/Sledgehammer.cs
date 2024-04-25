using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sledgehammer : Collectable
{
    Animator animator;
    [SerializeField] ChildTrigger trigger;
    Entity holder;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        handScale = Vector3.one * 100;
        handRotation = new Vector3(-96, 0, 90);
        handPosition = new Vector3(0, -0.29f, 0);
        interactText = "Pickup";
        animator = GetComponent<Animator>();
        trigger.TriggerEntered.AddListener(Hit);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Equip(Entity entity)
    {
        holder = entity;
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    public override void UnEquip(Entity entity)
    {

    }

    public override void Use(Entity entity, bool isDown)
    {
        if(isDown)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    void Hit(Collider other)
    {
        Entity entity;
        if(other.TryGetComponent<Entity>(out entity))
        {
            if(entity != holder)
            {
                Debug.Log("enemy hit");
                entity.Hit();
            }
            else
            {
                Debug.Log("hit self");
            }
        }
        else
        {
            Chain chain;
            if(other.TryGetComponent<Chain>(out chain))
            {
                chain.Break();
            }
        }
    }
}
