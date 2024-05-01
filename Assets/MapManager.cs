using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] Collider[] phase1;
    [SerializeField] Collider[] phase2;

    // Start is called before the first frame update
    void Start()
    {
        GM.I.loadPhase1.AddListener(EnablePhase1);
        GM.I.loadPhase2.AddListener(EnablePhase2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePhase1()
    {
        for(int i = 0; i < phase2.Length; i++)
        {
            phase2[i].enabled = false;
        }

        for(int i = 0; i < phase1.Length; i++)
        {
            phase1[i].enabled = true;
        }
    }

    public void EnablePhase2()
    {
        for (int i = 0; i < phase1.Length; i++)
        {
            phase1[i].enabled = false;
        }

        for (int i = 0; i < phase2.Length; i++)
        {
            phase2[i].enabled = true;
        }
    }
}
