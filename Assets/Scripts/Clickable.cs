using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clickable : Interactable
{
    protected override void Start()
    {
        base.Start();
        gameObject.tag = "Clickable";
        interactText = "Click";
    }
}
