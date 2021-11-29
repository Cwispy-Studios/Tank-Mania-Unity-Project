using System;
using CwispyStudios.TankMania.Enemy;
using CwispyStudios.TankMania.Player;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Enemy
{
  [RequireComponent(typeof(AiMovementController))]
  public class DroneEnemy : MonoBehaviour
  {
    [SerializeField] private float desiredHeight;
    [HideInInspector] public Rigidbody rb;

    private AiMovementController mc;
    private GunController gc;

    private void Awake()
    {
      mc = GetComponent<AiMovementController>();
      gc = GetComponentInChildren<GunController>();

      // TODO TEMP
      rb = GetComponent<Rigidbody>();
      rb.AddForce(Vector3.forward, ForceMode.Acceleration);

      InvokeRepeating(nameof(Folley), 1, 3);
    }

    private void Folley()
    {
      for (int i = 0; i < 10; i++)
      {
        gc.OnMainFire();
      }
    }

    public void ApplyForce(Vector3 force)
    {
      mc.ApplyMovementForce(force);

      if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit)) return;

      mc.ApplyMovementForce(Vector3.up * (desiredHeight - hit.distance));
    }
    
    public void SetMaxSpeed(float maxSpeed)
    {
      mc.SetMovementSpeed(maxSpeed);
    }
  }
}