using System.Collections;
using UnityEngine;

public class KamlaAI : MonoBehaviour
{
    public Transform player;
    public float initialSpeed = 2.0f;
    public float detectionRange = 10.0f;
    public float fieldOfViewAngle = 110f;  // Field of view in degrees
    public float speedIncrease = 0.5f;
    public float chaseDuration = 5.0f;  // Time to continue chasing after losing sight
    public float catchDistance = 1.5f;  // Distance at which Kamla catches the player
    public float runAwaySpeed = 10.0f;
    public float runAwayDuration = 3.0f;

    public Transform[] waypoints;  // Array of waypoints
    private int currentWaypointIndex = 0;
    private bool reverseOrder = false;  // To handle reverse order

    private float currentSpeed;
    private int itemsPlaced = 0;
    private bool isChasing = false;
    private bool isRunningAway = false;
    private Coroutine chaseCoroutine;
    private Coroutine runAwayCoroutine;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private float initialY;  // Store the initial Y position
    private Animator animator;  // Animator component
    private Collider colliderComponent; // Collider component

    private const int maxItems = 5; // Define maxItems here

    private void Start()
    {
        currentSpeed = initialSpeed;
        characterController = GetComponent<CharacterController>();
        initialY = transform.position.y;  // Set initial Y position
        animator = GetComponent<Animator>();  // Get Animator component
        colliderComponent = GetComponent<Collider>();  // Get Collider component
        StartCoroutine(EnableAfterDelay(50.0f));  // Delay Kamla's activation
    }

    private IEnumerator EnableAfterDelay(float delay)
    {
        DisableKamla();  // Disable Kamla's components
        yield return new WaitForSeconds(delay);
        EnableKamla();  // Enable Kamla's components
        MoveToNextWaypoint();  // Start Kamla's patrol
    }

    private void DisableKamla()
    {
        characterController.enabled = false;
        animator.enabled = false;
        colliderComponent.enabled = false;
    }

    private void EnableKamla()
    {
        characterController.enabled = true;
        animator.enabled = true;
        colliderComponent.enabled = true;
    }

    private void Update()
    {
        if (!characterController.enabled)
            return;

        if (isRunningAway)
            return;

        if (IsPlayerInSight())
        {
            if (chaseCoroutine != null)
            {
                StopCoroutine(chaseCoroutine);
                chaseCoroutine = null;
            }
            isChasing = true;
        }

        if (isChasing)
        {
            ChasePlayer();

            if (Vector3.Distance(transform.position, player.position) <= catchDistance)
            {
                CatchPlayer();
                return;
            }

            if (!IsPlayerInSight() && chaseCoroutine == null)
            {
                chaseCoroutine = StartCoroutine(ChaseForDuration(chaseDuration));
            }
        }
        else
        {
            Patrol();
        }
    }

    private bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + transform.up, directionToPlayer.normalized, out hit, detectionRange))
            {
                if (hit.collider.transform == player)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        moveDirection = direction * currentSpeed;
        moveDirection.y = 0;  // Ensure Y axis is constant
        characterController.Move(moveDirection * Time.deltaTime);
        MaintainGroundLevel();
    }

    private void CatchPlayer()
    {
        isChasing = false;
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
            chaseCoroutine = null;
        }
        if (runAwayCoroutine != null)
        {
            StopCoroutine(runAwayCoroutine);
        }
        runAwayCoroutine = StartCoroutine(RunAwayFromPlayer());
    }

    private IEnumerator RunAwayFromPlayer()
    {
        isRunningAway = true;
        Vector3 directionAway = (transform.position - player.position).normalized;
        float runAwayEndTime = Time.time + runAwayDuration;

        while (Time.time < runAwayEndTime)
        {
            moveDirection = directionAway * runAwaySpeed;
            moveDirection.y = 0;  // Ensure Y axis is constant
            characterController.Move(moveDirection * Time.deltaTime);
            MaintainGroundLevel();
            yield return null;
        }

        isRunningAway = false;
        MoveToNextWaypoint();
    }

    private IEnumerator ChaseForDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isChasing = false;
        MoveToNextWaypoint();
    }

    public void OnItemPlaced()
    {
        itemsPlaced++;
        currentSpeed += speedIncrease;

        if (itemsPlaced >= maxItems)
        {
            // Handle max items placed logic here if needed
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        moveDirection = direction * currentSpeed;
        moveDirection.y = CalculateVerticalSpeed(targetWaypoint.position);  // Calculate vertical movement

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentSpeed);

        characterController.Move(moveDirection * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        if (!reverseOrder)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                reverseOrder = true;
                currentWaypointIndex = waypoints.Length - 1;
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                reverseOrder = false;
                currentWaypointIndex = 0;
            }
        }
    }

    private float CalculateVerticalSpeed(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        return (targetPosition.y - transform.position.y) / distance * currentSpeed;
    }

    private void MaintainGroundLevel()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Lerp(position.y, initialY, Time.deltaTime * currentSpeed);  // Adjust Y position gradually
        transform.position = position;
    }
}
