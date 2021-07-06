using UnityEngine;
using UnityEngine.InputSystem;

namespace CwispyStudios.TankMania.Tank
{
  [RequireComponent(typeof(Rigidbody))]
  public class TankMovementController : MonoBehaviour
  {
    [Header("Acceleration")]
    [SerializeField, Range(2f, 5f)] private float maxVelocity = 3f;
    [SerializeField, Range(5f, 25f)] private float accelerationForce = 15f;

    [Header("Steering")]
    [SerializeField, Range(1f, 5f)] private float maxTorque = 1.5f;
    [SerializeField, Range(0.5f, 5f)] private float steerForce = 1.5f;
    [SerializeField, Range(1f, 5f)] private float steerNullifierForceModifier = 3.5f;

    private Rigidbody physicsController;

    private float maxVelocitySqr;
    private float accelerationInput = 0f;
    private float steerInput = 0f;
    private float torqueForce = 0f;

    private void Awake()
    {
      physicsController = GetComponent<Rigidbody>();

      maxVelocitySqr = maxVelocity * maxVelocity;
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
        torqueForce += steerInput * steerForce * Time.deltaTime;
        torqueForce = Mathf.Clamp(torqueForce, -maxTorque, maxTorque);
      }

      else
      {
        if (torqueForce != 0f)
        {
          torqueForce = Mathf.MoveTowards(torqueForce, 0f, steerForce * steerNullifierForceModifier * Time.deltaTime);
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
          physicsController.AddForce(transform.forward * accelerationInput * accelerationForce, ForceMode.Acceleration);
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
