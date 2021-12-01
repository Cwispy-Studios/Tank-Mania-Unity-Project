using UnityEngine;
using UnityEngine.AI;

namespace CwispyStudios.TankMania.Enemy
{
  [RequireComponent(typeof(Rigidbody))]
  public class PhysicsNavMeshAgent : AIMovementController
  {
    [Header("Debug options")] [SerializeField]
    private bool showPath;

    private NavMeshPath currentPath;
    private int currentPathIndex;
    private bool movingOnPath;

    protected override void Awake()
    {
      base.Awake();
      currentPath = new NavMeshPath();
    }

    // TODO make this dependent on the size of the collider
    private const float MinCornerDistance = .1f;

    public void StartPath(Vector3 newPosition)
    {
      bool hasPath = NavMesh.CalculatePath(physicsController.position, newPosition, NavMesh.AllAreas, currentPath);

      if (!hasPath)
      {
        movingOnPath = false;
        return;
      }

      if (showPath)
      {
        for (int i = 0; i < currentPath.corners.Length - 1; i++)
        {
          Debug.DrawLine(currentPath.corners[i], currentPath.corners[i + 1], Color.green, 1, false);
          Debug.DrawRay(currentPath.corners[i], Vector3.up * 2, Color.blue, 1);
          Debug.DrawRay(currentPath.corners[i + 1], Vector3.up * 2, Color.blue, 1);
        }
      }

      //Debug.Log("Calculated new path");

      currentPathIndex = 0;
      movingOnPath = true;
    }

    public void StopPath()
    {
      movingOnPath = false;
    }

    protected override void FixedUpdate()
    {
      if (!movingOnPath) return;
      
      base.FixedUpdate();
      ApplyMovementForce(DirectionToNextCorner(), ForceMode.VelocityChange);
      UpdatePath();
    }

    private void UpdatePath()
    {
      Vector3 position = physicsController.position;
      position.y = 0;
      Vector3 waypoint = currentPath.corners[currentPathIndex];
      waypoint.y = 0;

      if (Vector3.Distance(position, waypoint) < MinCornerDistance)
      {
        //Debug.Log("Changed waypoint");
        currentPathIndex++;
      }

      if (currentPathIndex == currentPath.corners.Length)
      {
        //Debug.Log("Reached destination");
        StopPath();
      }
    }

    private Vector3 DirectionToNextCorner()
    {
      if (currentPath.corners.Length == 0) return Vector3.zero;
      Vector3 direction = (currentPath.corners[currentPathIndex] - transform.position);
      direction.y = 0;
      return direction.normalized;
    }
  }
}