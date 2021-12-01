using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  // Prototype behavior for a grounded tank-like enemy which will follow the player around and try to deal melee damage
  public class MeleePrototype : MonoBehaviour
  {
    [SerializeField] private float minDistance;

    private Transform followedTransform; // Transform to be followed by the attached tank
    
    private PhysicsNavMeshAgent movementController;
    private MeleeController meleeController;

    private bool following = true; // True if player is outside of the attacking range

    private bool Following
    {
      get => following;
      set
      {
        if (following == value && !following)
          return;
        following = value;
        if (!following)
          movementController.StopPath();
        else
        {
          movementController.StartPath(followedTransform.position);
        }
      }
    }

    void Start()
    {
      movementController = GetComponent<PhysicsNavMeshAgent>();
      meleeController = GetComponent<MeleeController>();

      followedTransform = GameObject.FindGameObjectWithTag("Player").transform;
      
      Scheduler.Instance.GetTimer(.2f) += FollowTransform;
      
      movementController.StartPath(followedTransform.position);
    }

    private void FollowTransform()
    {
      if (Vector3.Distance(followedTransform.position, transform.position) > minDistance)
      {
        Following = true;
      }
      else
      {
        Following = false;
        meleeController.MeleeAttack(0);
      }
    }
  }
}