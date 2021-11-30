using CwispyStudios.TankMania.Stats;
using Unity.Mathematics;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  public class AiMovementController : MonoBehaviour
  {
    [Header("Agent movement parameters")] [SerializeField]
    protected AiMovementStats aiMovementStats;

    protected Rigidbody physicsController;

    private float forceFactor = 100;

    protected virtual void Awake()
    {
      physicsController = GetComponent<Rigidbody>();
    }

    public void ApplyMovementForce(Vector3 direction)
    {
      Vector3 velocity = physicsController.velocity;
      velocity.y = 0f;

      Vector3 force = aiMovementStats.UsesAcceleration ? direction * aiMovementStats.AccelerationForce.Value : direction;
      if ((velocity + Time.deltaTime * forceFactor * force).magnitude <= aiMovementStats.MaxVelocity.Value)
      {
        physicsController.AddForce(Time.deltaTime * forceFactor * force, ForceMode.Acceleration);
      }

      if (!aiMovementStats.RotatesWithForce || direction == Vector3.zero) return;
      Quaternion newRotation =
        Quaternion.RotateTowards(physicsController.rotation,
          quaternion.LookRotation(direction.normalized, Vector3.up), aiMovementStats.TurningSpeed.Value * 1000 * Time.deltaTime);
      physicsController.MoveRotation(newRotation);
    }
  }
}