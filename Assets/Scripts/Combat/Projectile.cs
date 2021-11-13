using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;
  using Visuals;

  public class Projectile : MonoBehaviour
  {
    // Particle system/vfx to play when projectile impacts
    [SerializeField] private CFX_AutoDestructShuriken explosionVfx = null;

    [Header("Custom Gravity")]
    [SerializeField] private bool usesCustomGravity = false;
    [SerializeField, Range(0, -100f), Tooltip("Default gravity is -9.81")] 
    private float customGravityValue;

    // Used to turn the projectile invisible while leaving its vfx running
    private MeshRenderer meshRenderer;
    // Prevents OnEnable from running when being instantiated
    private bool disableOnEnabled = true;
    // Prevents multiple collisions and hits from being registered at once
    private bool disableProjectileCollisions = false;

    // Damage from the object firing the projectile
    private Damage damageInformation;

    // Cache accessible rigidbody
    [HideInInspector] public Rigidbody PhysicsController;

    private void Awake()
    {
      meshRenderer = GetComponent<MeshRenderer>();
      PhysicsController = GetComponent<Rigidbody>();

      disableOnEnabled = true;

      explosionVfx.OnDeactivate += Deactivate;

      if (usesCustomGravity) PhysicsController.useGravity = false;
    }

    private void OnEnable()
    {
      if (disableOnEnabled) { disableOnEnabled = false; return; }

      BulletEvents.BulletFired(this);
    }

    private void FixedUpdate()
    {
      if (usesCustomGravity) PhysicsController.AddForce(0f, customGravityValue, 0f, ForceMode.Acceleration);
    }

    private void Update()
    {
      transform.LookAt(PhysicsController.position + PhysicsController.velocity);
    }

    private void OnCollisionEnter( Collision collision )
    {
      if (disableProjectileCollisions) return;

      HandleProjectileCollison(collision);
    }

    private void HandleProjectileCollison( Collision collision )
    {
      GameObject collisionObject = collision.gameObject;
      Vector3 collisionPoint = collision.GetContact(0).point;

      damageInformation.DamageObject(collisionObject);
      damageInformation.HandleSplashDamageIfEnabled(collisionObject, collisionPoint);

      // Moves the explosion vfx to the point of contact and enables it
      explosionVfx.transform.position = collisionPoint;
      explosionVfx.gameObject.SetActive(true);

      // Deactivates physics of the projectile
      PhysicsController.detectCollisions = false;
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.Discrete;
      PhysicsController.isKinematic = true;

      // Deactivates projectile from colliding again before physics is disabled
      disableProjectileCollisions = true;

      // Turns the projectile invisible
      meshRenderer.enabled = false;

      BulletEvents.BulletHit(this);
    }

    private void Deactivate()
    {
      // Reenables physics
      PhysicsController.isKinematic = false;
      PhysicsController.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      PhysicsController.velocity = Vector3.zero;
      PhysicsController.detectCollisions = true;

      // Turns the projectile visible
      meshRenderer.enabled = true;

      // Resets damage information
      damageInformation = null;

      // Return to object pool
      gameObject.SetActive(false);

      disableProjectileCollisions = false;
    }

    public void SetDamage( Damage damage )
    {
      damageInformation = damage;
    }
  }
}
