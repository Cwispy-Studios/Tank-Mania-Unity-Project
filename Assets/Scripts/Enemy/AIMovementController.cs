using Unity.Mathematics;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using Stats;
  
  // ReSharper disable once InconsistentNaming
  [RequireComponent(typeof(Rigidbody))]
  public class AIMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    protected AIMovementControllerStats aiMovementControllerStats;
    [SerializeField] private bool usesAcceleration;
    [SerializeField] private bool rotatesWithForce;

    protected Rigidbody physicsController;

    private float squaredMaxVelocity;

    protected virtual void Awake()
    {
      physicsController = GetComponent<Rigidbody>();

      UpdateSquaredMaxVelocity();

      aiMovementControllerStats.MaxVelocity.OnStatUpgrade += UpdateSquaredMaxVelocity;
    }

    private void UpdateSquaredMaxVelocity()
    {
      squaredMaxVelocity = aiMovementControllerStats.MaxVelocity.Value * aiMovementControllerStats.MaxVelocity.Value;
    }

    public void ApplyMovementForce(Vector3 direction, ForceMode forceMode)
    {
      Vector3 force = usesAcceleration
        ? direction * aiMovementControllerStats.AccelerationForce.Value
        : direction;

      physicsController.AddForce(force, forceMode);

      if (physicsController.velocity.sqrMagnitude > squaredMaxVelocity)
        physicsController.velocity = Vector3.ClampMagnitude(physicsController.velocity, aiMovementControllerStats.MaxVelocity.Value);
    }

    protected virtual void FixedUpdate()
    {
      if (!rotatesWithForce) return;
      
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