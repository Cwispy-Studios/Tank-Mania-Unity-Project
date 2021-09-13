using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace CwispyStudios.TankMania.Enemy
{
  [RequireComponent(typeof(NavMeshAgent))]
  [RequireComponent(typeof(Rigidbody))]
  public class AIMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    private bool
      usePhysics; // Acts as override for now, makes it so that we switch to the custom nav mesh implementation

    [SerializeField] [Range(0.1f, 10.0f)] private float movementSpeed;

    [SerializeField] [Range(0.1f, 5.0f)] private float accelerationSpeed;

    [SerializeField] [Range(0.1f, 5.0f)] private float turningSpeed;

    // Nav mesh agent instance used for movement controls.
    private NavMeshAgent navMeshAgent;
    private NavMeshPath currentPath;
    private int currentPathIndex;
    private bool movingOnPath;

    private Rigidbody physicsController;

    private void Awake()
    {
      if (usePhysics)
      {
        currentPath = new NavMeshPath();
        physicsController = GetComponent<Rigidbody>();
      }
      else
      {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;
        navMeshAgent.acceleration = accelerationSpeed;
        navMeshAgent.angularSpeed = turningSpeed;
      }
    }

    // NON PHYSICS IMPLEMENTATION----------------------------------------------------------------------------------

    public void EnablePhysics()
    {
      InterruptMovement();
      navMeshAgent.enabled = false;
    }

    public void DisablePhysics()
    {
      navMeshAgent.enabled = true;
    }

    public void SetMovementParams(float movementSpeed, float accelerationSpeed, float turningSpeed)
    {
      navMeshAgent.speed = movementSpeed;
      navMeshAgent.acceleration = accelerationSpeed;
      navMeshAgent.angularSpeed = turningSpeed;
    }

    public void MoveToPosition(Vector3 newPosition)
    {
      navMeshAgent.SetDestination(newPosition);
    }

    public void InterruptMovement()
    {
      navMeshAgent.SetDestination(transform.position);
    }

    //-------------------------------------------------------------------------------------------------------------

    // TODO make this dependent on the size of the collider
    private float minCornerDistance = .1f;

    public void StartPath(Vector3 newPosition)
    {
      bool hasPath = NavMesh.CalculatePath(physicsController.position, newPosition, NavMesh.AllAreas, currentPath);

      if (!hasPath) return;
      
      Debug.Log("Calculated new path");

      currentPathIndex = 0;
      movingOnPath = true;
    }

    public void StopPath()
    {
      movingOnPath = false;
    }

    // TODO @Cwispy Currently this implementations borrows a lot from the TankMovementController physics implementation
    // Let me know if we should refactor this
    private void FixedUpdate()
    {
      if (!movingOnPath) return;
      
      UpdatePhysics();
      UpdatePath();
    }

    private void UpdatePath()
    {
      Vector3 position = physicsController.position;
      position.y = 0;
      Vector3 waypoint = currentPath.corners[currentPathIndex];
      waypoint.y = 0;

      if (Vector3.Distance(position, waypoint) < minCornerDistance)
      {
        Debug.Log("Changed waypoint");
        currentPathIndex++;
      }

      if (currentPathIndex == currentPath.corners.Length)
      {
        Debug.Log("Reached destination");
        StopPath();
      }
    }

    // TODO this is pretty much the same as accelerate @Cwispy
    // Except it doesn't work
    private void UpdatePhysics()
    {
      Vector3 velocity = physicsController.velocity;
      velocity.y = 0f;

      if (velocity.sqrMagnitude <= movementSpeed)
      {
        Vector3 force = DirectionToNextCorner() * accelerationSpeed;
        physicsController.AddForce(force * 10, ForceMode.Acceleration);
      }
    }

    private Vector3 DirectionToNextCorner()
    {
      Vector3 direction = (currentPath.corners[currentPathIndex] - transform.position).normalized;
      direction.y = 0;
      return direction;
    }
  }
}