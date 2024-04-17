using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clickable : Interactable
{
    public override string interactText { get { return "Click"; } protected set { } }

    protected override void Start()
    {
        base.Start();
        gameObject.tag = "Clickable";
    }
}
