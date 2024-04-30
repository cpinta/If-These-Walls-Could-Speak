using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Phase1_BackInTheBedroomOneTimeAudio : OneTimeAudio
{
    [SerializeField] Door door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }


    protected override void TriggerActivated()
    {
        GM.I.Phase1_BackInTheBedroom();
        GM.I.DisableGrandma();
    }
}