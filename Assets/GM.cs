using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM I
    {
        get
        {
            return instance;
        }
    }
    private static GM instance;

    public float distanceToDestination = 0.25f;
    public bool debug = true;

    public string[] safePossibilites = {
        "DARK",     // house is dark / tape is dark / wife has gone dark
        "DEMN",     // demon
        "CLEO",     // wife's name
        "ABBA",     // favorite band
        "MISS",     // as in "I miss you"
        "HELP",     // help
    };

    public string safeAnswer = "";

    public List<int> clockSequence = new List<int>();
    int clockSequenceLength = 6;

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


        StartGame();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {
        safeAnswer = safePossibilites[Random.Range(0, GM.I.safePossibilites.Length)];
        Debug.Log("Safe answer: " + safeAnswer);
        GenerateClockSequence();
    }

    void GenerateClockSequence()
    {
        string sequenceString = "";
        for(int i = 0; i < clockSequenceLength; i++)
        {
            int rand = Random.Range(0, 12);
            while (clockSequence.Contains(rand))
            {
                rand = Random.Range(0, 12);
            }
            clockSequence.Add(rand);
        }
        clockSequence.Sort();
        for(int i = 0;i < clockSequenceLength;i++)
        {
            sequenceString += clockSequence[i] + " ";
        }
        Debug.Log("Clock Sequence set to: "+sequenceString);
    }

    //used for testing to make sure the clock worked correctly on all hours
    void ClockTestSequence()
    {
        for (int i = 0; i < 12; i++)
        {
            clockSequence.Add(i);
        }
        string sequenceString = "";
        for (int i = 0; i < 12; i++)
        {
            sequenceString += clockSequence[i] + " ";
        }
        Debug.Log("Clock Sequence set to: " + sequenceString);
    }
}
