using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

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

    public MapManager mapManager;
    public PlatePlacementManager platePlacementManager;
    public FridgeLetterManager fridgeLetterManager;
    public Radio radio;
    [SerializeField] PlayerController player;
    [SerializeField] Grandma grandma;

    public UnityEvent clockSolved;
    public UnityEvent onePlateCorrect;
    public UnityEvent loadPhase1;
    public UnityEvent loadPhase2;

    [SerializeField] GameObject uiBlackScreen;

    [SerializeField] CutsceneManager cutsceneManager;
    [SerializeField] TimelineAsset scenePlayerGrabbed;
    [SerializeField] TimelineAsset scenePhase1_WokeUp;
    [SerializeField] TimelineAsset sceneGrandmaInFridge1;
    [SerializeField] TimelineAsset sceneGrandmaInFridge2;
    [SerializeField] TimelineAsset scenePhase1_BackInTheRoom; 
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

        cutsceneManager.cutsceneDone.AddListener(CutsceneFinished);

        StartGame();
        LoadPhase1();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerGrabbed()
    {
        cutsceneManager.PlayCutscene(scenePlayerGrabbed);
    }

    public void MakePlayerSpeak(AudioClip clip)
    {
        player.PlaySound(clip);
    }

    void StartGame()
    {
        uiBlackScreen.SetActive(false);
        safeAnswerIndex = Random.Range(0, GM.I.safePossibilites.Length);
        safeAnswer = safePossibilites[safeAnswerIndex];
        radio.SetAnswer(safeAnswerRadio[safeAnswerIndex]);

        Debug.Log("Safe answer: " + safeAnswer);
        GenerateClockSequence();
    }

    public void ResetGame()
    {
        Entity[] entity = FindObjectsOfType<Entity>();
        for(int i= 0; i < entity.Length; i++)
        {
            entity[i].ResetGame();
        }

        Interactable[] interactables = FindObjectsOfType<Interactable>();
        for(int i= 0;i < interactables.Length; i++)
        {
            interactables[i].ResetGame();
        }
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

    public void LoadPhase1()
    {
        player.canJaunt = false;
        grandma.DisableGrandma();
        cutsceneManager.PlayCutscene(scenePhase1_WokeUp);
        AddMessageToFridge("luv u sweetie");
        player.canInteract = false;
    }

    public void Phase1_PlayerEnteredKitchen()
    {
        player.canJaunt = true;
        cutsceneManager.PlayCutscene(sceneGrandmaInFridge2);
    }

    public void Phase1_BackInTheBedroom()
    {
        cutsceneManager.PlayCutscene(scenePhase1_BackInTheRoom);
    }

    public void DisableGrandma()
    {
        grandma.ChangeState(GrandmaState.Dormant);
    }

    void CutsceneFinished(PlayableAsset playableAsset)
    {
        if(playableAsset == scenePhase1_WokeUp)
        {
            cutsceneManager.PlayCutscene(sceneGrandmaInFridge1);
        }
        else if(playableAsset == sceneGrandmaInFridge1)
        {

        }
        else if(playableAsset == sceneGrandmaInFridge2)
        {
            //grandma.SetTarget(player);
            //grandma.ChangeState(GrandmaState.Chasing);
            grandma.EnableGrandma();
        }
        else if(playableAsset == scenePhase1_BackInTheRoom)
        {
            cutsceneManager.StopCutscene();
            DisableGrandma();
            AddMessageToFridge("u must escape");
            player.canInteract = true;
        }
    }

    public void AddMessageToFridge(string message)
    {
        fridgeLetterManager.AddMessageToQueue(message);
    }
}
