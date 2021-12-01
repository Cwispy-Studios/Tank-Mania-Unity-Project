using System;
using CwispyStudios.TankMania.Enemy;
using CwispyStudios.TankMania.Player;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Enemy
{
  [RequireComponent(typeof(AIMovementController))]
  public class BirdoEnemy : MonoBehaviour
  {
    public Rigidbody rb;

    private AIMovementController mc;
    private GunController gc;

    private void Start()
    {
      mc = GetComponent<AIMovementController>();
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
        gc.QueueFiring();
      }
    }

    public void ApplyForce(Vector3 force, float maxSpeed)
    {
      if (rb.velocity.magnitude < maxSpeed)
        rb.AddForce(force, ForceMode.Acceleration);
      //mc.StartPath(rb.position + force);
    }
  }
}