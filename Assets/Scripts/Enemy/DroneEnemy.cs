using System;
using CwispyStudios.TankMania.Player;
using CwispyStudios.TankMania.Stats;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  [RequireComponent(typeof(AIMovementController))]
  public class DroneEnemy : MonoBehaviour
  {
    private const float DesiredHeight = 10f;
    private const float HeightChangeFactor = .05f;
    
    [HideInInspector] public Rigidbody rb; // used by DroneFlock for calculations

    private AIMovementController mc;
    private GunController gc;

    private void Awake()
    {
      mc = GetComponent<AIMovementController>();
      gc = GetComponentInChildren<GunController>();
      rb = GetComponent<Rigidbody>();

      Scheduler.Instance.GetTimer(3) += Folley;
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
      mc.ApplyMovementForce(force, ForceMode.VelocityChange);

      if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit)) return;

      mc.ApplyMovementForce(Vector3.up * ((DesiredHeight - hit.distance) * HeightChangeFactor), ForceMode.VelocityChange);
    }
  }
}