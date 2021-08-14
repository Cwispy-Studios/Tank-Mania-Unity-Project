using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  [RequireComponent(typeof(Rigidbody))]
  public class TankMovementController : MonoBehaviour
  {
    [SerializeField] private PlayerMovement playerMovement;

    // Cache rigidbody component
    private Rigidbody physicsController;

    // Cache calcuation
    private float maxVelocitySqr;
    // W-S input
    private float accelerationInput = 0f;
    // A-D input
    private float steerInput = 0f;
    // Current torque force acting on the tank
    private float torqueForce = 0f;

    private void Awake()
    {
      physicsController = GetComponent<Rigidbody>();

      maxVelocitySqr = playerMovement.MaxVelocity.Value * playerMovement.MaxVelocity.Value;
    }

    ///////////////////
    // Unity callbacks

    private void Update()
    {
      UpdateTorque();
    }

    private void FixedUpdate()
    {
      Steer();
      Accelerate();
    }

    private void UpdateTorque()
    {
      if (steerInput != 0f)
      {
        torqueForce += steerInput * playerMovement.SteerForce.Value * Time.deltaTime;
        torqueForce = Mathf.Clamp(torqueForce, -playerMovement.MaxTorque.Value, playerMovement.MaxTorque.Value);
      }

      else
      {
        if (torqueForce != 0f)
        {
          float maxDelta = playerMovement.SteerForce.Value * playerMovement.SteerNullifierForce.Value * Time.deltaTime;
          torqueForce = Mathf.MoveTowards(torqueForce, 0f, maxDelta);
        }
      }
    }

    private void Steer()
    {
      if (torqueForce != 0f)
      {
        Quaternion deltaRotation = Quaternion.Euler(0f, torqueForce, 0f);
        physicsController.MoveRotation(physicsController.rotation * deltaRotation);
      }
    }

    private void Accelerate()
    {
      if (accelerationInput != 0f)
      {
        // Ignore horizontal velocity
        Vector3 velocity = physicsController.velocity;
        velocity.y = 0f;

        if (velocity.sqrMagnitude <= maxVelocitySqr)
        {
          Vector3 force = transform.forward * accelerationInput * playerMovement.AccelerationForce.Value;
          physicsController.AddForce(force, ForceMode.Acceleration);
        }
      }
    }

    ///////////////////////////
    // Input Actions callbacks

    private void OnAccelerate( InputValue value )
    {
      accelerationInput = value.Get<float>();
    }

    private void OnSteer( InputValue value )
    {
      steerInput = value.Get<float>();
    }
  }
}
