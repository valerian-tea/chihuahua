using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Chasing,
    Attacking,
}

public class EnemyController : MonoBehaviour
{
    // private static readonly int isMoving = Animator.StringToHash("IsMoving");

    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private bool isWaiting;
    private float timeSinceLostSight;
    private bool isAttacking;
    private EnemyState currentState = EnemyState.Patrolling;

    [Header("References")]
    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private AttackInput attackInput; // Reference to the hitbox/damage system

    [Header("Settings")]
    [SerializeField]
    private float patrolWaitTime = 2f;

    [SerializeField]
    private float detectionRadius = 5f;

    [SerializeField]
    private float losePlayerTime = 3f;

    [SerializeField]
    private float attackRange = 0.5f;

    [SerializeField]
    private float attackCooldown = 3f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GoNextPatrolPoint();
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRadius && CanSeePlayer())
                {
                    currentState = EnemyState.Chasing;
                }
                break;
            case EnemyState.Chasing:
                Chase();
                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
                if (!CanSeePlayer())
                {
                    timeSinceLostSight += Time.deltaTime;
                    if (timeSinceLostSight >= losePlayerTime)
                    {
                        currentState = EnemyState.Patrolling;
                    }
                }
                else
                {
                    timeSinceLostSight = 0f;
                }
                break;
            case EnemyState.Attacking:
                if (!isAttacking)
                {
                    if (distanceToPlayer > attackRange)
                    {
                        currentState = EnemyState.Chasing;
                        agent.isStopped = false;
                    }
                    else
                    {
                        Attack();
                    }
                }
                break;
        }
        // UpdateAnimations();
    }

    private void Attack()
    {
        agent.isStopped = true;
        var directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;
        if (directionToPlayer != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer); //look at player
        }
        isAttacking = true;
        Debug.Log("Attacking the player!");
        animator.SetTrigger("Attack");
        StartCoroutine(WaitForAttackAnimation());
    }

    IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
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

    private bool CanSeePlayer()
    {
        return IsFacingPlayer() && HasLineOfSightToPlayer();
    }

    private bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        return dotProduct > 0.4f; // Adjust threshold as needed
    }

    private bool HasLineOfSightToPlayer()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
        {
            return hit.transform == player;
        }
        return true;
    }
}
