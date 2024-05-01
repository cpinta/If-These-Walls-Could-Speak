using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sledgehammer : Collectable
{
    Animator animator;
    [SerializeField] ChildTrigger trigger;
    Entity holder;
    [SerializeField] AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        handScale = Vector3.one;
        handRotation = new Vector3(25.8f, 90, -15);
        handPosition = new Vector3(-0.38f, -0.29f, 0.12f);
        interactText = "Pickup";
        animator = GetComponent<Animator>();
        trigger.TriggerEntered.AddListener(Hit);
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        if (beingManipulated)
        {
            transform.localPosition = handPosition;
            transform.localRotation = Quaternion.Euler(handRotation);
            beingManipulated = false;
        }
    }

    public override void Equip(Entity entity)
    {
        GM.I.MakePlayerSpeak(clip);
        holder = entity;
        animator.SetBool("InHand", true);
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

            ChainInteract chainInt;
            if (other.TryGetComponent<ChainInteract>(out chainInt))
            {
                chainInt.Break();
            }
        }
    }

    public void Fall()
    {
        animator.SetBool("Fall", true);
    }

    public override void ResetGame()
    {
        base.ResetGame();

        animator.SetBool("InHand", false);
        animator.SetBool("Fall", false);
    }
}
