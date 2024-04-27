using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public Clock clock;

    public float distanceToDestination = 0.25f;
    public bool debug = true;

    public string[] safePossibilites = {
        "SAVE",     // save my wife pwease
        "DEMN",     // demon
        "CLEO",     // wife's name
        "ABBA",     // favorite band
        "MISS",     // as in "I miss you"
        "HELP",     // help
    };

    public AudioClip[] safeAnswerRadio;
    public int safeAnswerIndex = 0;

    public string safeAnswer = "";

    public List<int> clockSequence = new List<int>();
    int clockSequenceLength = 6;

    public PlatePlacementManager platePlacementManager;
    public Radio radio;

    public UnityEvent clockSolved;
    public UnityEvent onePlateCorrect;

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

        radio = FindObjectOfType<Radio>();

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
        safeAnswerIndex = Random.Range(0, GM.I.safePossibilites.Length);
        safeAnswer = safePossibilites[safeAnswerIndex];
        radio.SetAnswer(safeAnswerRadio[safeAnswerIndex]);

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