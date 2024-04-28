using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

enum GrandmaState
{
    Dormant = 0,
    Wandering = 1,
    Standing = 2,
    Chasing = 3,
}

enum CurrentDirection
{
    Clockwise = 0,
    CounterClockwise = 1,
}

public class Grandma : Entity
{
    Entity target;
    Entity[] potentialTargets;
    [SerializeField] GrandmaState gmaState;
    bool firstStateAfterSpawn;
    CurrentDirection currentDirection;
    GmaDestination[] destinations;
    GmaDestination destination;              //used when wandering around the house. Ignored if player is found
    [SerializeField] Transform face;
    NavMeshAgent navAgent;
    Animator animator;
    AudioSource audioSource;

    RaycastHit hit;
    float sightDistance = 100;

    float checkFOVeverySecs = 0.2f;
    float cantFindPlayerTime = 5; //Time it takes for Grandma to stop chasing the Player when she doesnt see them for a while
    float cantFindPlayerTimer = 0;

    float standingTimeMin = 1;
    float standingTimeMax = 2;
    float standingTimer = 0;
    float switchDirectionChance = 0.1f;

    float wanderingTimeMin = 5;
    float wanderingTimeMax = 10;
    float wanderingTimer = 0;
    float destinationArrivedDistance = 2;
    bool wanderingAtDestination = false;

    float wanderingOutOfSightTime = 3;
    float wanderingOutOfSightTimer = 0;

    float nextSpawnTimeMin = 10;
    float nextSpawnTimeMax = 30;
    float nextSpawnTimer = 0;

    private Coroutine coroutineFOVRoutine;

    Vector3 dormantPostion = new Vector3(0, -500, 0);

    [SerializeField] float groundRaycastOffset;
    [SerializeField] float groundRaycastDistance;


    private Vector2 velocity;
    Vector2 smoothDeltaPostion;

    string FoundPlayer = "Found Player";
    string HitByPlayer = "Hit By Player";
    string KillingPlayer = "Killing Player";
    string LookingForPlayer = "Looking for Player";
    string PlayerLeaving = "Player Leaving";
    string Spawned = "Spawned";

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        destinations = FindObjectsOfType<GmaDestination>();
        potentialTargets = FindObjectsOfType<Entity>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.applyRootMotion = true;
        navAgent.updatePosition = false;
        navAgent.updateRotation = true;
        //ChangeState(GrandmaState.Standing);

        for (int i = 0; i < destinations.Length; i++)
        {
            if(destinations[i].name == "Gma Destination Upstairs")
            {
                destination = destinations[i];
                break;
            }
        }
        gmaState = GrandmaState.Wandering;
        navAgent.destination = destination.transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        SyncAnimatorAndAgent();
        if (gmaState == GrandmaState.Standing || gmaState == GrandmaState.Wandering)
        {
            if(coroutineFOVRoutine == null)
            {
                coroutineFOVRoutine = StartCoroutine(FOVRoutine());
            }

            RaycastHit wanderingHit;
            Physics.Raycast(transform.position, potentialTargets[0].transform.position, out wanderingHit);
            if(wanderingHit.collider == null)
            {
                if (wanderingOutOfSightTimer > 0)
                {
                    wanderingOutOfSightTimer -= Time.deltaTime;
                }
                else
                {
                    ChangeState(GrandmaState.Dormant);
                }
            }
            else
            {
                wanderingOutOfSightTimer = wanderingOutOfSightTime;
            }
        }

        if(gmaState == GrandmaState.Standing)
        {
            if(standingTimer > 0)
            {
                standingTimer -= Time.deltaTime;
            }
            else
            {
                ChangeState(GrandmaState.Wandering);
            }
        }

        if (gmaState == GrandmaState.Wandering)
        {
            float distanceToDestination = Vector3.Distance(transform.position, destination.transform.position);
            if (distanceToDestination > destinationArrivedDistance)
            {
                if(wanderingTimer > 0)
                {
                    wanderingTimer -= Time.deltaTime;
                }
                else
                {
                    wanderingAtDestination = false;
                    ChangeState(GrandmaState.Standing);
                }
            }
            else
            {
                wanderingAtDestination = true;
                if (destination.isEndpoint)
                {
                    ChangeState(GrandmaState.Dormant);
                }
                else
                {
                    ChangeState(GrandmaState.Standing);
                }
            }
        }

        if(gmaState == GrandmaState.Chasing)
        {
            navAgent.destination = target.transform.position;
            if (cantFindPlayerTimer > 0)
            {
                Physics.Raycast(face.transform.position, target.transform.forward, out hit, sightDistance);
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player")
                    {
                        Debug.DrawRay(face.transform.position, target.transform.position, Color.green);
                        cantFindPlayerTimer = cantFindPlayerTime;
                    }
                    else
                    {
                        cantFindPlayerTime -= Time.deltaTime;
                    }
                }
                else
                {
                    Debug.DrawRay(face.transform.position, target.transform.position, Color.red);
                    cantFindPlayerTime -= Time.deltaTime;
                }
            }
            else
            {
                gmaState = GrandmaState.Wandering;
            }
        }

        if(gmaState == GrandmaState.Dormant)
        {
            transform.position = dormantPostion;
            if (nextSpawnTimer > 0)
            {
                nextSpawnTimer -= Time.deltaTime;
            }
            else
            {
                StartLurking();
            }
        }
    }

    void StartLurking()
    {
        destination = destinations[Random.Range(0, destinations.Length)];

        //checking which destinations the player can see. If they cant see it, Grandma will spawn there
        List<GmaDestination> potentialDests = new List<GmaDestination>();
        for(int i = 0; i < destinations.Length; i++)
        {
            RaycastHit destHit;
            Physics.Raycast(potentialTargets[0].transform.position, destination.transform.forward, out destHit);
            if(destHit.collider != null)
            {
                potentialDests.Add(destinations[i]);
            }
        }
        if(potentialDests.Count == 0)
        {
            nextSpawnTimer = 3;
            return;
        }
        destination = potentialDests[Random.Range(0, potentialDests.Count)];

        // 50/50 if going clockwise or not
        if (Random.value > 0.5f)                    //Going clockwise
        {
            currentDirection = CurrentDirection.Clockwise;
        }
        else                                        //Going counter-clockwise
        {
            currentDirection = CurrentDirection.CounterClockwise;
        }

        destination = GetNextDestination(destination);

        firstStateAfterSpawn = true;
        // 50/50 if starting standing or wandering
        if (Random.value > 0.5f)
        {
            ChangeState(GrandmaState.Standing);
        }
        else
        {
            ChangeState(GrandmaState.Wandering);
        }

        PlayRandomSoundFromFolder(Spawned, true);
    }

    void SyncAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothDeltaPostion = Vector2.Lerp(smoothDeltaPostion, deltaPosition, smooth);

        velocity = smoothDeltaPostion / Time.deltaTime;
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            velocity = Vector2.Lerp(Vector2.zero, velocity, navAgent.remainingDistance / navAgent.stoppingDistance);
        }

        bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.stoppingDistance;

        animator.SetBool("Move", shouldMove);
        animator.SetFloat("Locomotion", velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > navAgent.radius / 2)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, navAgent.nextPosition, smooth);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = navAgent.nextPosition.y;
        transform.position = rootPosition;
        navAgent.nextPosition = rootPosition;
    }


    void ChangeState(GrandmaState state)
    {
        gmaState = state;
        switch (state)
        {
            case GrandmaState.Standing:
                standingTimer = Random.Range(standingTimeMin, standingTimeMax);
                navAgent.isStopped = true;
                animator.SetBool("Sprint", false);
                if(!firstStateAfterSpawn)
                {
                    PlayRandomSoundFromFolder(LookingForPlayer, true);
                }
                firstStateAfterSpawn = false;
                break;
            case GrandmaState.Wandering:
                navAgent.isStopped = false;
                if(destination == null)
                {
                    destination = destinations[Random.Range(0, destinations.Length)];
                }

                if (wanderingAtDestination)
                {
                    if (Random.value < switchDirectionChance)
                    {
                        currentDirection = currentDirection == CurrentDirection.Clockwise ? CurrentDirection.CounterClockwise : CurrentDirection.Clockwise;
                    }

                    destination = GetNextDestination(destination);

                    navAgent.destination = destination.transform.position;
                }
                wanderingTimer = Random.Range(wanderingTimeMin, wanderingTimeMax);
                animator.SetBool("Sprint", false);
                break;
            case GrandmaState.Dormant:
                navAgent.isStopped = true;
                transform.position = dormantPostion;
                nextSpawnTimer = Random.Range(nextSpawnTimeMin, nextSpawnTimeMax);
                animator.SetBool("Sprint", false);
                break;
            case GrandmaState.Chasing:
                navAgent.isStopped = false;
                navAgent.destination = target.transform.position;
                cantFindPlayerTimer = cantFindPlayerTime;
                animator.SetBool("Sprint", true);
                PlayRandomSoundFromFolder(FoundPlayer, true);
                break;
        }
    }

    void PlayRandomSoundFromFolder(string folderName, bool interruptCurrent)
    {
        if (audioSource.isPlaying)
        {
            if (!interruptCurrent)
            {
                return;
            }
        }

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Voicelines/Grandma/" + folderName);
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();

        //AudioClip clip = Resources.Load<AudioClip>("Voicelines/Grandma/Found Player/come here i missed you");
        //audioSource.clip = clip;
    }

    GmaDestination GetNextDestination(GmaDestination dest)
    {
        if (currentDirection == CurrentDirection.Clockwise)
        {
            GmaDestination newDest = dest.GetNext();
            if (newDest == null)
            {
                currentDirection = CurrentDirection.CounterClockwise;
                return dest.GetPrev();
            }
            else
            {
                return newDest;
            }
        }
        else
        {
            GmaDestination newDest = dest.GetPrev();
            if (newDest == null)
            {
                currentDirection = CurrentDirection.Clockwise;
                return dest.GetNext();
            }
            else
            {
                return newDest;
            }
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(checkFOVeverySecs);

        while (gmaState != GrandmaState.Dormant || gmaState != GrandmaState.Chasing)
        {
            yield return wait;
            target = FieldOfViewCheck();
            if (target != null)
            {
                ChangeState(GrandmaState.Chasing);
                break;
            }
        }
    }



    void PlayerRaycast()
    {
        switch (gmaState)
        {
            case GrandmaState.Standing:

                break;
            case GrandmaState.Dormant: 
                
                break;
            case GrandmaState.Wandering: 
                
                break;
            case GrandmaState.Chasing:
                break;
        }
    }
}