using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace CwispyStudios.TankMania.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovementController : MonoBehaviour
    {
        [Header("Agent movement parameters")]
        
        [SerializeField]
        [Range(0.1f, 10.0f)]
        private float movementSpeed;

        [SerializeField] 
        [Range(0.1f, 5.0f)]
        private float accelerationSpeed;

        [SerializeField] 
        [Range(0.1f, 5.0f)] 
        private float turningSpeed;
        
        // Nav mesh agent instance used for movement controls.
        NavMeshAgent navMeshAgent;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = movementSpeed;
            navMeshAgent.acceleration = accelerationSpeed;
            navMeshAgent.angularSpeed = turningSpeed;
        }

        public void EnablePhysics()
        {
            InterruptMovement();
            navMeshAgent.enabled = false;
        }

        public void DisablePhysics()
        {
            navMeshAgent.enabled = true;
        }

        public void SetMovementParams(float movementSpeed, float accelerationSpeed, float turningSpeed)
        {
            navMeshAgent.speed = movementSpeed;
            navMeshAgent.acceleration = accelerationSpeed;
            navMeshAgent.angularSpeed = turningSpeed;
        }

        public void MoveToPosition(Vector3 newPosition)
        {
            navMeshAgent.SetDestination(newPosition);
        }

        public void InterruptMovement()
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }
}
