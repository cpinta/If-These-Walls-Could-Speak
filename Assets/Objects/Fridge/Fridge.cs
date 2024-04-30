using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] FridgeLetterManager letterManager;
    Renderer rend;
    bool rendered = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rendered)
        {
            if(!rend.isVisible)
            {
                rendered = false;
            }
        }
    }


    private void OnWillRenderObject()
    {
        if (!rendered)
        {
            letterManager.DisplayLatestQueueInMessage();
            rendered = true;
        }
    }
}
