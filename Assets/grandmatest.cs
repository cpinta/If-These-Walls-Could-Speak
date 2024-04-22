using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class grandmatest : MonoBehaviour
{
    NavMeshAgent navAgent;
    Animator animator;

    private Vector2 velocity;
    Vector2 smoothDeltaPostion;

    Vector3 dest1 = new Vector3(-9.96000004f, 0.163000003f, 7.10000038f);
    Vector3 dest2 = new Vector3(-4.09299994f, 0.163000003f, 13.191f);
    [SerializeField] Vector3 currentDest = Vector3.zero;

    float destinationTimer = 10;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.applyRootMotion = true;
        navAgent.updatePosition = false;
        navAgent.updateRotation = true;

        currentDest = dest1;
        
                navAgent.destination = currentDest;
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SyncAnimatorAndAgent();

        if(navAgent.remainingDistance < 1 )
        {
            if (currentDest == dest1)
            {
                currentDest = dest2;
                navAgent.destination = currentDest;
            }
            else
            {
                currentDest = dest1;
                navAgent.destination = currentDest;
            }
        }
        else
        {
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = navAgent.nextPosition.y;
        transform.position = rootPosition;
        navAgent.nextPosition = rootPosition;
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
        if(navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            velocity = Vector2.Lerp(Vector2.zero, velocity, navAgent.remainingDistance / navAgent.stoppingDistance);
        }

        bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.stoppingDistance;

        animator.SetBool("Move", shouldMove);
        animator.SetFloat("Locomotion", velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if(deltaMagnitude > navAgent.radius / 2)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, navAgent.nextPosition, smooth);
        }
    }
}
