using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningRoomDoor : Door
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        GM.I.loadPhase1.AddListener(Phase1);
        GM.I.loadPhase2.AddListener(Phase2);
    }

    void Phase1()
    {
        locked = true;
        isOpen = true;
        Close();
    }

    void Phase2()
    {
        locked = false;
        isOpen = false;

        Open();
    }
}
