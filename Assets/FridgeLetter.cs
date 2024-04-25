using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeLetter : MonoBehaviour
{
    MeshFilter meshFilter;
    Mesh currentMesh;

    bool isAtDestination;
    Vector3 destination = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter.mesh = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAtDestination)
        {

        }
    }

    public bool SetLetter(char letter)
    {
        currentMesh = Resources.Load<Mesh>("Letters/"+letter.ToString().ToLower());
        if(currentMesh != null)
        {
            meshFilter.mesh = currentMesh;
            Debug.Log("FridgeLetter: fridge letter "+letter.ToString()+" loaded to "+this.name);
            return true;
        }
        Debug.Log("FridgeLetter: couldn't load fridge letter " + letter.ToString() + " for " + this.name);
        return false;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        isAtDestination = false;
    }
}
