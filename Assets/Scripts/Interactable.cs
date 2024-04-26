using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual string interactText { get { return _interactText; } protected set { _interactText = value; } }
    string _interactText;
    public virtual bool interactInput { get { return true; } protected set { } }
    public Collider collider;


    protected virtual void Start()
    {
        gameObject.tag = "Interactable";
        if(collider == null)
        {
            collider = GetComponent<Collider>();
        }
    }

    public abstract void Interact(Entity entity);

    public abstract void IsHovering(bool isHovering, Entity entity);

    public abstract void ResetGame();
}