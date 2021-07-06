using UnityEngine;

namespace CwispyStudios.TankMania.Projectile
{
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

    private void Update()
    {
      transform.LookAt(PhysicsController.position + PhysicsController.velocity);
    }

    private void OnCollisionEnter( Collision collision )
    {
      explosionVfx.transform.position = collision.GetContact(0).point;
      explosionVfx.gameObject.SetActive(true);

      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Discrete;
      PhysicsController.isKinematic = true;

      meshRenderer.enabled = false;
    }

    private void Deactivate()
    {
      PhysicsController.isKinematic = false;
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Continuous;
      PhysicsController.velocity = Vector3.zero;

      meshRenderer.enabled = true;

      gameObject.SetActive(false);
    }
  }
}
