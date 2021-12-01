using CwispyStudios.TankMania.Stats;
using Unity.Mathematics;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  // ReSharper disable once InconsistentNaming
  public class AIMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    protected AiMovementStats aiMovementStats;

    protected Rigidbody physicsController;

    private float forceFactor = 100;

    protected virtual void Awake()
    {
      physicsController = GetComponent<Rigidbody>();
    }

<<<<<<< HEAD
    // TODO make this dependent on the size of the collider
    private float minCornerDistance = .1f;

    public void StartPath(Vector3 newPosition)
    {
      bool hasPath = NavMesh.CalculatePath(physicsController.position, newPosition, NavMesh.AllAreas, currentPath);

      if (!hasPath) return;

      if (showPath)
      {
        for (int i = 0; i < currentPath.corners.Length - 1; i++)
          Debug.DrawLine(currentPath.corners[i], currentPath.corners[i + 1], Color.green, 1, false);
      }
      
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
        //Debug.Log("Changed waypoint");
        currentPathIndex++;
      }

      if (currentPathIndex == currentPath.corners.Length)
      {
        //Debug.Log("Reached destination");
        StopPath();
      }
    }

    // TODO this is pretty much the same as Accelerate in TankMovementController @Cwispy
    private void UpdatePhysics()
=======
    public void ApplyMovementForce(Vector3 direction)
>>>>>>> AI-Implementation
    {
      Vector3 velocity = physicsController.velocity;
      velocity.y = 0f;

      Vector3 force = aiMovementStats.UsesAcceleration ? direction * aiMovementStats.AccelerationForce.Value : direction;
      if ((velocity + Time.deltaTime * forceFactor * force).magnitude <= aiMovementStats.MaxVelocity.Value)
      {
        physicsController.AddForce(Time.deltaTime * forceFactor * force, ForceMode.Acceleration);
        //print(Time.deltaTime * forceFactor * force);
      }

      if (!aiMovementStats.RotatesWithForce || direction == Vector3.zero) return;
      Quaternion newRotation =
        Quaternion.RotateTowards(physicsController.rotation,
          quaternion.LookRotation(direction.normalized, Vector3.up), aiMovementStats.TurningSpeed.Value * 1000 * Time.deltaTime);
      physicsController.MoveRotation(newRotation);
    }
  }
}