using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimationParent : MonoBehaviour
{
    Animator animator;
    Collider collider;
    Book book;
    bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        book = GetComponentInChildren<Book>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void BookFell()
    {
        book.EnableGrabbing();
        animator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!activated)
            {
                animator.SetTrigger("Fall");
                activated = true;
                collider.enabled = false;
            }
        }
    }

    public void ResetGame()
    {
        activated = true;
        collider.enabled = true;
        animator.enabled = true;
    }
}
