using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using GameEvents;
  using Player;

  [RequireComponent(typeof(AIMovementController))]
  public class DroneEnemy : MonoBehaviour
  {
    [Header("Events")]
    [Tooltip("Notifies the flocking manager to manage this drone.")]
    [SerializeField] private DroneEnemyEvent onDroneSpawn;
    [Tooltip("Notifies the flocking manager to stop managing this drone.")]
    [SerializeField] private DroneEnemyEvent onDroneDespawn;

    private const float DesiredHeight = 15f;
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

    private void OnEnable()
    {
      onDroneSpawn.Raise(this);
    }

    private void OnDisable()
    {
      onDroneDespawn.Raise(this);
    }

    private void Folley()
    {
      for (int i = 0; i < 10; i++)
      {
        gc.QueueFiring();
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