using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
    public class AIMeleeDebugger : MonoBehaviour
    {
        [SerializeField] private Transform followedTransform;
        [SerializeField] private float minDistance;

        private AIMovementController mct;
        private MeleeEnemy meleeEnemy;

        private bool following = true;

        public bool Following
        {
            get => following;
            set
            {
                if (following == value)
                    return;
                following = value;
                if (following)
                    mct.DisablePhysics();
                else
                    mct.EnablePhysics();
                print("stop");
            }
        }

        void Start()
        {
            mct = GetComponent<AIMovementController>();
            meleeEnemy = GetComponent<MeleeEnemy>();
            InvokeRepeating(nameof(FollowTransform), 0, .1f);
        }

        private void FollowTransform()
        {
            if (Vector3.Distance(followedTransform.position, transform.position) > minDistance)
            {
                Following = true;
                mct.MoveToPosition(followedTransform.position);
            }
            else
            {
                Following = false;
                meleeEnemy.MeleeAttack(0, 10);
            }
        }
    }
}
