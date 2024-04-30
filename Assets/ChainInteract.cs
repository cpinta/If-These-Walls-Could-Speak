using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainInteract : Interactable
{
    bool hasBeenInteractedWith;
    [SerializeField] AudioClip clip;

    public override void Interact(Entity entity)
    {
        if(!hasBeenInteractedWith)
        {
            GM.I.MakePlayerSpeak(clip);
            GM.I.AddMessageToFridge("get the key to his room");
            hasBeenInteractedWith = true;
        }
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        hasBeenInteractedWith = false;
        interactText = "Interact";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
