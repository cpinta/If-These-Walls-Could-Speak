using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I
    {
        get
        {
            return instance;
        }
    }
    private static GameManager instance;

    public float distanceToDestination = 0.25f;
    public bool debug = true;

    public string[] safePossibilites = {
        "DARK",     // house is dark / tape is dark / wife has gone dark
        "HOUS",     // house
        "CLEO",     // wife's name
        "ABBA",     // favorite band
        "SAFE",     // opening the safe / "we are unsafe"
        "HELP",     // help
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("More than one GameManager in scene!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
