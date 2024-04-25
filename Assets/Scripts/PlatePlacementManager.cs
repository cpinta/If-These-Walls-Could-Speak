using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatePlacementManager : MonoBehaviour
{
    public static PlatePlacementManager I
    {
        get
        {
            return instance;
        }
    }
    private static PlatePlacementManager instance;

    List<PlatePlacement> platePlacements = new List<PlatePlacement>();


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("More than one PlatePlacementManager in scene!");
            Destroy(this);
        }

        platePlacements = GetComponentsInChildren<PlatePlacement>().ToList();
        for(int i = 0; i < platePlacements.Count; i++)
        {
            platePlacements[i].platePlaced.AddListener(CheckPlate);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPlate(PlatePlacement platePlacement)
    {
        if((platePlacement.HasPlate() && GM.I.clockSequence.Contains(platePlacements.IndexOf(platePlacement))) || (!platePlacement.HasPlate() && !GM.I.clockSequence.Contains(platePlacements.IndexOf(platePlacement))))
        {
            if (CheckAnswer())
            {
                GM.I.clockSolved.Invoke();
            }
            else
            {
                GM.I.onePlateCorrect.Invoke();
            }
        }
    }

    public bool CheckAnswer()
    {
        for (int i = 0; i < platePlacements.Count; i++)
        {
            if ((platePlacements[i].HasPlate() && GM.I.clockSequence.Contains(i)) || (!platePlacements[i].HasPlate() && !GM.I.clockSequence.Contains(i)))
            {

            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
