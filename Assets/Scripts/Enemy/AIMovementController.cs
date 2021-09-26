using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace CwispyStudios.TankMania.Enemy
{
  [RequireComponent(typeof(Rigidbody))]
  public class AIMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] 

    [SerializeField] private bool rotateWithPath;

    [SerializeField] [Range(0.1f, 10.0f)] private float movementSpeed;

    [SerializeField] [Range(0.1f, 5.0f)] private float accelerationSpeed;

    [SerializeField] [Range(0.1f, 5.0f)] private float turningSpeed;

    private NavMeshPath currentPath;
    private int currentPathIndex;
    private bool movingOnPath;

    private Rigidbody physicsController;

    private void Awake()
    {
      currentPath = new NavMeshPath();
        physicsController = GetComponent<Rigidbody>();
    }

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

    // TODO this is pretty much the same as Accelerate in TankMovementController @Cwispy
    private void UpdatePhysics()
    {
      Vector3 velocity = physicsController.velocity;
      velocity.y = 0f;

      Vector3 direction = DirectionToNextCorner();

      if (velocity.sqrMagnitude <= movementSpeed)
      {
        Vector3 force = direction * accelerationSpeed;
        physicsController.AddForce(force * 10, ForceMode.Acceleration);
      }

      if (rotateWithPath && direction != Vector3.zero)
      {
        Quaternion newRotation =
        Quaternion.RotateTowards(physicsController.rotation,
          quaternion.LookRotation(direction, Vector3.up), turningSpeed);
        physicsController.MoveRotation(newRotation);
      }
    }

    private Vector3 DirectionToNextCorner()
    {
      Vector3 direction = (currentPath.corners[currentPathIndex] - transform.position);
      direction.y = 0;
      return direction.normalized;
    }
  }
}