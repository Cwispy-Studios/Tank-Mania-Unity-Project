using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  public class AIMeleeDebugger : MonoBehaviour
  {
    [SerializeField] private Transform followedTransform;
    [SerializeField] private float minDistance;

    private AIMovementController mct;
    private MeleeController meleeController;

    private bool following = true;

    public bool Following
    {
      get => following;
      set
      {
        if (following == value)
          return;
        following = value;
        if (!following)
          mct.StopPath();
        else
        {
          mct.StartPath(followedTransform.position);
        }

        print("stop");
      }
    }

    void Start()
    {
      mct = GetComponent<AIMovementController>();
      meleeController = GetComponent<MeleeController>();
      InvokeRepeating(nameof(FollowTransform), 0, .2f);
      
      mct.StartPath(followedTransform.position);
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
        meleeController.MeleeAttack(0, 10);
      }
    }
  }
}