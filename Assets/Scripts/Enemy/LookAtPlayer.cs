using System;
using UnityEngine;

namespace Enemy
{
    public class LookAtPlayer : MonoBehaviour
    {
        private Transform playerTransform;
        
        // Start is called before the first frame update
        void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            transform.LookAt(playerTransform);
        }
    }
}
