using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeAudio : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    protected bool wasActived = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(wasActived)
        {
            return;
        }

        if(other.tag == "Player")
        {
            if(clip != null)
            {
                GM.I.MakePlayerSpeak(clip);
            }
            TriggerActivated();
            wasActived = true;
        }
    }

    protected virtual void TriggerActivated()
    {

    }

    public virtual void ResetGame()
    {
        wasActived = false;
    }
}
