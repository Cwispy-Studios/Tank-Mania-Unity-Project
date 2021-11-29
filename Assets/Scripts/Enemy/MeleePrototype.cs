using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  public class MeleePrototype : MonoBehaviour
  {
    [SerializeField] private Transform followedTransform;
    [SerializeField] private float minDistance;

    private PhysicsNavMeshAgent movementController;
    private MeleeController meleeController;

    private bool following = true;

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

        print("stop");
      }
    }

    void Start()
    {
      movementController = GetComponent<PhysicsNavMeshAgent>();
      meleeController = GetComponent<MeleeController>();
      InvokeRepeating(nameof(FollowTransform), 0, .2f);
      
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