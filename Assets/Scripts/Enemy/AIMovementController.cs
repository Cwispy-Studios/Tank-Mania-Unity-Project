using System;
using CwispyStudios.TankMania.Stats;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace CwispyStudios.TankMania.Enemy
{
  // ReSharper disable once InconsistentNaming
  [RequireComponent(typeof(Rigidbody))]
  public class AIMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    protected AIMovementControllerStats aiMovementControllerStats;

    protected Rigidbody physicsController;

    private float squaredMaxVelocity;

    protected virtual void Awake()
    {
      physicsController = GetComponent<Rigidbody>();

      squaredMaxVelocity = Mathf.Pow(aiMovementControllerStats.MaxVelocity.Value, 2f);
    }

    public void ApplyMovementForce(Vector3 direction, ForceMode forceMode)
    {
      Vector3 force = aiMovementControllerStats.UsesAcceleration
        ? direction * aiMovementControllerStats.AccelerationForce.Value
        : direction;

      physicsController.AddForce(force, forceMode);

      if (physicsController.velocity.sqrMagnitude > squaredMaxVelocity)
        physicsController.velocity = physicsController.velocity.normalized * aiMovementControllerStats.MaxVelocity.Value;

      if (!aiMovementControllerStats.RotatesWithForce || direction == Vector3.zero) return;
    }

    protected virtual void FixedUpdate()
    {
      Vector3 turnVector = physicsController.velocity;
      turnVector.y = 0;
      turnVector.Normalize();
      
      if (turnVector.sqrMagnitude == 0) return;

      Quaternion newRotation =
        Quaternion.RotateTowards(physicsController.rotation,
          quaternion.LookRotation(turnVector, Vector3.up),
          aiMovementControllerStats.TurningSpeed.Value);
      physicsController.MoveRotation(newRotation);
    }
  }
}