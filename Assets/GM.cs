using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
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
    [SerializeField] TimelineAsset sceneLightsOff;

    [SerializeField] GameObject phase2Colliders;
    [SerializeField] Light[] lights;

    [SerializeField] Sledgehammer sledgehammer;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Camera playerGrabCamera;

    [SerializeField] OneTimeAudio[] oneTimeAudios;

    int phase = 1;


    bool despawningGrandma = false;
    float grandmaDespawnTimer = 5;
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
        oneTimeAudios = FindObjectsByType<OneTimeAudio>(FindObjectsSortMode.None);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (despawningGrandma)
        {
            if (grandmaDespawnTimer > 0)
            {
                grandmaDespawnTimer -= Time.deltaTime;
            }
            else
            {
                despawningGrandma = false;
                DisableGrandma();
            }
        }
    }

    public void PlayerGrabbed()
    {
        cutsceneManager.PlayCutscene(scenePlayerGrabbed);
        Cursor.lockState = CursorLockMode.None;
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

    public void LoadPhase2()
    {
        phase2Colliders.SetActive(true);
        loadPhase2.Invoke();
        phase = 2;
    }

    public void RespawnPlayer()
    {
        playerGrabCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        cutsceneManager.StopCutscene();
        gameOverScreen.SetActive(false);
        player.Respawn();

        for(int i =0; i < oneTimeAudios.Length; i++)
        {
            oneTimeAudios[i].ResetGame();
        }

        if (phase == 1)
        {
            LoadPhase1();
            grandma.transform.position = new Vector3(-6.335f, 0.136f, 1.766f);
            grandma.ChangeState(GrandmaState.Standing);
        }
        if(phase == 2)
        {
            LoadPhase2();
            grandma.ChangeState(GrandmaState.Dormant);
        }
        player.transform.position = new Vector3(-8.345f, 1.017f, 20.764f);
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 235, 0));

    }

    public void LoadPhase1()
    {
        phase2Colliders.SetActive(false);
        player.canJaunt = false;
        grandma.DisableGrandma();
        cutsceneManager.PlayCutscene(scenePhase1_WokeUp);
        AddMessageToFridge("luv u sweetie");
        player.canInteract = false;
        //player.canInteract = true;
        player.SetSpeed(2);
        loadPhase1.Invoke();
    }

    public void Phase1_PlayerEnteredKitchen()
    {
        player.canJaunt = true;
        grandma.GameStart();
        cutsceneManager.PlayCutscene(sceneGrandmaInFridge2);
        player.ResetSpeed();
    }

    public void Phase1_BackInTheBedroom()
    {
        cutsceneManager.PlayCutscene(scenePhase1_BackInTheRoom);
        grandma.SetCanSprint(true);
        despawningGrandma = true;
        grandmaDespawnTimer = 1.5f;
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
            AddMessageToFridge("u must escape");
            player.canInteract = true;
            LoadPhase2();
        }
    }

    public void AddMessageToFridge(string message)
    {
        fridgeLetterManager.AddMessageToQueue(message);
    }

    public void CutsceneTurnOffLights()
    {
        cutsceneManager.PlayCutscene(sceneLightsOff);
        TurnOffLights();
    }

    public void TurnOffLights()
    {
        for(int i=0; i < lights.Length; i++)
        {
            lights[i].enabled = false;

        }
    }
    public void TurnOnLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;

        }
    }



    public void OutroTapePlaying(CameraSpot cameraSpot)
    {
        player.Hide(cameraSpot, false, true);
        player.pointLight.enabled = false;
        player.flashlight.enabled = false;
        DisableGrandma();
        grandma.canWander = false;
    }

    public void OutroTapeDone()
    {
        player.StartExitingHide();
        sledgehammer.Fall();
        player.pointLight.enabled = true;
        player.flashlight.enabled = true;
        grandma.canWander = true;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("End", LoadSceneMode.Single);
    }
}
