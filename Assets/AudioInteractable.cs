using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInteractable : Interactable
{
    [SerializeField] AudioClip clip;

    public override void Interact(Entity entity)
    {
        GM.I.MakePlayerSpeak(clip);
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
