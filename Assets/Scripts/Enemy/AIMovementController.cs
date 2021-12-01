using System;
using CwispyStudios.TankMania.Stats;
using Unity.Mathematics;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  // ReSharper disable once InconsistentNaming
  [RequireComponent(typeof(Rigidbody))]
  public class AIMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    protected AiMovementStats aiMovementStats;

    protected Rigidbody physicsController;

    private float squaredMaxVelocity;

    protected virtual void Awake()
    {
      physicsController = GetComponent<Rigidbody>();

      squaredMaxVelocity = Mathf.Pow(aiMovementStats.MaxVelocity.Value, 2f);
    }

    public void ApplyMovementForce(Vector3 direction, ForceMode forceMode)
    {
      Vector3 force = aiMovementStats.UsesAcceleration
        ? direction * aiMovementStats.AccelerationForce.Value
        : direction;

      physicsController.AddForce(force, forceMode);

      if (physicsController.velocity.sqrMagnitude > squaredMaxVelocity)
        physicsController.velocity = physicsController.velocity.normalized * aiMovementStats.MaxVelocity.Value;

      if (!aiMovementStats.RotatesWithForce || direction == Vector3.zero) return;
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
          aiMovementStats.TurningSpeed.Value);
      physicsController.MoveRotation(newRotation);
    }
  }
}