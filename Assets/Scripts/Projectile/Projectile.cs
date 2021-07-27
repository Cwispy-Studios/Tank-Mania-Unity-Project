using UnityEngine;

namespace CwispyStudios.TankMania.Projectile
{
  using Visuals;

  public class Projectile : MonoBehaviour
  {
    [SerializeField] private CFX_AutoDestructShuriken explosionVfx = null;

    private MeshRenderer meshRenderer;

    [HideInInspector] public Rigidbody PhysicsController;

    private void Awake()
    {
      meshRenderer = GetComponent<MeshRenderer>();
      PhysicsController = GetComponent<Rigidbody>();

      explosionVfx.OnDeactivate += Deactivate;
    }

    private void OnEnable()
    {
      BulletEvents.BulletFired(this);
    }

    private void Update()
    {
      transform.LookAt(PhysicsController.position + PhysicsController.velocity);
    }

    private void OnCollisionEnter( Collision collision )
    {
      // Moves the explosion vfx to the point of contact and enables it
      explosionVfx.transform.position = collision.GetContact(0).point;
      explosionVfx.gameObject.SetActive(true);

      // Deactivates physics of the projectile
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Discrete;
      PhysicsController.isKinematic = true;

      // Turns the projectile invisible
      meshRenderer.enabled = false;
      
      BulletEvents.BulletHit(this);
    }

    private void Deactivate()
    {
      // Reenables physics
      PhysicsController.isKinematic = false;
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Continuous;
      PhysicsController.velocity = Vector3.zero;

      // Turns the projectile visible
      meshRenderer.enabled = true;

      // Return to object pool
      gameObject.SetActive(false);
    }
  }
}
