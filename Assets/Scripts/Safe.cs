using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Safe : CameraSpot
{
    Animator animator;
    [SerializeField] SafeDial[] dials;
    bool isActive = false;
    bool locked = true;
    Entity currentUnlocker;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        syncRotation = true;
        isCursorLocked = false;
        TurnOffDials();
        SetDials();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            AttemptUnlock();
        }
    }

    public override void Interact(Entity entity)
    {
        if (locked)
        {
            base.Interact(entity);
            currentUnlocker = entity;
            TurnOnDials();
        }
        else        // if unlocked
        {

        }
    }

    public override void UnHide()
    {
        base.UnHide();
        TurnOffDials();
        currentUnlocker = null;
    }

    public override void IsHovering(bool isHovering, Entity entity)
    {

    }

    void TurnOffDials()
    {
        for(int i = 0; i < dials.Length; i++)
        {
            dials[i].collider.enabled = false;
        }
    }
    void TurnOnDials()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].collider.enabled = true;
        }
    }

    void SetDials()
    {
        string[] temp = new string[GM.I.safePossibilites.Length];
        Array.Copy(GM.I.safePossibilites, temp, temp.Length);

        List<int> indexes = new List<int>();
        for (int i = 0; i<temp.Length; i++)
            indexes.Add(i);

        for (int i = 0; i < dials.Length; i++)
        {
            List<int> tempIndexes = new List<int>(indexes);
            for (int j = 0; j < 6; j++)
            {
                int rand = Random.Range(0, tempIndexes.Count);
                dials[i].SetLetter(j, temp[tempIndexes[rand]][i]);
                tempIndexes.RemoveAt(rand);
            }
        }
    }

    void AttemptUnlock()
    {
        if (IsLockCorrect())
        {
            locked = false;
            animator.SetBool("Open", true);
            currentUnlocker.StartUnHide();
        }
    }

    bool IsLockCorrect()
    {
        for(int i=0; i < dials.Length; i++)
        {
            if (dials[i].CurrentLetter() != GM.I.safeAnswer[i])
            {
                return false;
            }
        }
        return true;
    }
}