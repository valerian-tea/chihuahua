using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// private enum EnemyState
// {
//     Idle,
//     Chasing,
//     Attacking,
// }

public class EnemyController : MonoBehaviour
{
    private static readonly int isMoving = Animator.StringToHash("IsMoving");

    [SerializeField]
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private bool isWaiting;

    [SerializeField]
    private Transform player;

    [Header("References")]
    [SerializeField]
    private Transform[] patrolPoints;

    [Header("Settings")]
    [SerializeField]
    private float patrolWaitTime = 2f;

    [SerializeField]
    private float detectionRadius = 5f;

    private void Update()
    {
        Patrol();
        UpdateAnimations();
    }

    private void Patrol()
    {
        if (isWaiting)
            return;
        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
            StartCoroutine(WaitAtPatrolPoint());
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(patrolWaitTime);
        agent.isStopped = false;
        GoNextPatrolPoint();
        isWaiting = false;
    }

    private void GoNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void UpdateAnimations()
    {
        var isCurrentlyMoving = agent.velocity.magnitude > 0.01f;
        animator.SetBool(isMoving, isCurrentlyMoving);
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GoNextPatrolPoint();
    }
}
