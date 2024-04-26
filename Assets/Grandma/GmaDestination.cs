using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GmaDestination : MonoBehaviour
{
    //next in the clockwise rotation of the house
    public GmaDestination[] next;
    //prev in the counter-clockwise rotation of the house
    public GmaDestination[] prev;

    public bool isEndpoint = false;

    public GmaDestination GetNext()
    {
        return GetDest(next);
    }

    public GmaDestination GetPrev()
    {
        return GetDest(prev);
    }

    GmaDestination GetDest(GmaDestination[] dests)
    {
        if (dests.Length == 0)
        {
            return null;
        }
        else if (dests.Length == 1)
        {
            return dests[0];
        }
        else
        {
            return dests[Random.Range(0, dests.Length)];
        }
    }
}
