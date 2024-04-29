using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfHallwayOneTimeAudio : OneTimeAudio
{
    [SerializeField] Collider backInRoomCollider;

    // Start is called before the first frame update
    void Start()
    {
        backInRoomCollider.gameObject.SetActive(false);
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
        backInRoomCollider.gameObject.SetActive(true);
        GM.I.Phase1_PlayerEnteredKitchen();
    }
}