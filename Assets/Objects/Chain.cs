using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] DoubleDoor[] doors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].ForceOpen();
        }
        Destroy(this);
    }
}
