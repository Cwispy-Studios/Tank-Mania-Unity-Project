using System;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy
{
  public class AiMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    protected bool rotateWithDirection;

    [SerializeField] private bool usesAcceleration;

    [SerializeField] [Range(0.1f, 10.0f)] protected float movementSpeed;

    [SerializeField] [Range(0.1f, 5.0f)] protected float accelerationSpeed;

    [SerializeField] [Range(0.1f, 5.0f)] protected float turningSpeed;

    protected Rigidbody physicsController;

    protected virtual void Awake()
    {
      physicsController = GetComponent<Rigidbody>();
    }

    public void SetMovementSpeed(float speed)
    {
      movementSpeed = speed;
    }

    public void ApplyMovementForce(Vector3 direction)
    {
      Vector3 velocity = physicsController.velocity;
      velocity.y = 0f;

      if (velocity.sqrMagnitude <= movementSpeed)
      {
        Vector3 force = usesAcceleration? direction * accelerationSpeed : direction;
        physicsController.AddForce(Time.deltaTime * 1000 * force, ForceMode.Acceleration);
        print("force");
      }

      if (!rotateWithDirection || direction == Vector3.zero) return;
      Quaternion newRotation =
        Quaternion.RotateTowards(physicsController.rotation,
          quaternion.LookRotation(direction.normalized, Vector3.up), turningSpeed * 1000 * Time.deltaTime);
      physicsController.MoveRotation(newRotation);
    }
  }
}