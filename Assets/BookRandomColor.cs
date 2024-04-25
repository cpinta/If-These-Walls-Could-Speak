using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BookRandomColor : MonoBehaviour
{
    GameObject[] children;

    // Start is called before the first frame update
    void Start()
    {
        Renderer[] meshes = GetComponentsInChildren<Renderer>();
        if(meshes != null)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].material.color = Color.HSVToRGB(Random.value, 1, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
