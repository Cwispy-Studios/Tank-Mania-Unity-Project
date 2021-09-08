using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
    public class AIMovementDebugger : MonoBehaviour
    {
        [SerializeField] private Transform followedTransform;

        private AIMovementController mct;
        
        void Start()
        {
            mct = GetComponent<AIMovementController>();
            InvokeRepeating(nameof(FollowTransform), 0, .1f);
        }

        private void FollowTransform()
        {
            mct.MoveToPosition(followedTransform.position);
        }
    }
}
