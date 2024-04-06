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
    GrandmaState gmaState;
    CurrentDirection currentDirection;
    GmaDestination[] destinations;
    GmaDestination destination;              //used when wandering around the house. Ignored if player is found
    [SerializeField] Transform face;
    NavMeshAgent navAgent;

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

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        destinations = FindObjectsOfType<GmaDestination>();
        potentialTargets = FindObjectsOfType<Entity>();
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

        // 50/50 if starting standing or wandering
        if (Random.value > 0.5f)
        {
            ChangeState(GrandmaState.Standing);
        }
        else
        {
            ChangeState(GrandmaState.Wandering);
        }
    }


    void ChangeState(GrandmaState state)
    {
        gmaState = state;
        switch (state)
        {
            case GrandmaState.Standing:
                standingTimer = Random.Range(standingTimeMin, standingTimeMax);
                navAgent.isStopped = true;
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
                break;
            case GrandmaState.Dormant:
                navAgent.isStopped = true;
                transform.position = dormantPostion;
                nextSpawnTimer = Random.Range(nextSpawnTimeMin, nextSpawnTimeMax);
                break;
            case GrandmaState.Chasing:
                navAgent.isStopped = false;
                navAgent.destination = target.transform.position;
                cantFindPlayerTimer = cantFindPlayerTime;
                break;
        }
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
            //target = FieldOfViewCheck();
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