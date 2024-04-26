using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeLetter : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;

    bool isAtDestination;
    Vector3 destination = Vector3.zero;

    float letterSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAtDestination)
        {
            float distance = Vector3.Distance(transform.localPosition, destination);
            if (Vector3.Distance(transform.localPosition, destination) > GM.I.distanceToDestination)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, Time.deltaTime * letterSpeed);
            }
            else
            {
                isAtDestination = true;
            }
        }
    }

    public bool SetLetter(char letter)
    {
        meshFilter.mesh = Resources.Load<Mesh>("Letters/"+letter.ToString().ToLower());
        if(meshFilter.mesh != null)
        {
            meshFilter.mesh = meshFilter.mesh;
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

    public void SetColor()
    {
        Renderer mesh = GetComponent<Renderer>();
        if (mesh != null)
        {
            mesh.material.color = Color.HSVToRGB(Random.value, 1, 1);
        }
    }
}
