using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using Stats;

  public class DroneFlock : MonoBehaviour
  {
    private Rigidbody targetRigidbody;

    [Header("MovementB behaviour, if offset is 0, flock will go directly to target without strafing")] [SerializeField]
    private float targetOffsetDistance;

    [SerializeField] private float targetStrokeDuration;

    private Vector3 rightOffsetVector;
    private float targetStrokeTimer = 0f;

    [Header("Force component parameters")] [SerializeField]
    private float neighbourDistance;

    [SerializeField] private float desiredSeparation;

    [SerializeField] private AIMovementControllerStats aiMovementControllerStats;

    [Header("Component weights")] [SerializeField]
    private float separationFactor = 1.5f;

    [SerializeField] private float alignFactor = 1f;
    [SerializeField] private float cohesionFactor = 1f;
    [SerializeField] private float seekFactor = 4f;

    private List<DroneEnemy> flockingDrones = new List<DroneEnemy>();

    private void Awake()
    {
      SetTarget(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>());

      rightOffsetVector = new Vector3(targetOffsetDistance, 0, 0);
    }

    // Can be called to have the flock target another entity;
    public void SetTarget(Rigidbody target)
    {
      targetRigidbody = target;
    }

    private Vector3 GetDynamicTargetPosition()
    {
      targetStrokeTimer += Time.deltaTime;

      if (targetStrokeTimer >= targetStrokeDuration)
      {
        targetStrokeTimer = 0;
      }
      
      Vector3 returnTarget =  targetRigidbody.position + targetRigidbody.velocity + targetRigidbody.transform.rotation *
                              Vector3.Lerp(rightOffsetVector, -rightOffsetVector, Mathf.PingPong(Time.time / targetStrokeDuration, 1f));
      
      Debug.DrawRay(returnTarget, Vector3.up, Color.red, 1);

      return returnTarget;
    }

    public void AddDrone(DroneEnemy drone)
    {
      flockingDrones.Add(drone);
    }

    public void RemoveDrone( DroneEnemy drone )
    {
      flockingDrones.Remove(drone);
    }

    private void FixedUpdate()
    {
      UpdateFlock();
    }

    private void UpdateFlock()
    {
      for (int i = 0; i < flockingDrones.Count; i++)
      {
        Vector3 sep = Separate(i); // Separation	
        Vector3 ali = Align(i); // Alignment	
        Vector3 coh = Cohesion(i); // Cohesion	
        Vector3 seek = Seek(i, GetDynamicTargetPosition());

        // Arbitrarily weight these forces	
        sep *= separationFactor;
        ali *= alignFactor;
        coh *= cohesionFactor;
        seek *= seekFactor;

        // Add the force vectors to acceleration	
        flockingDrones[i].ApplyForce(sep + ali + coh + seek);
      }
    }

    private void LimitVector(ref Vector3 vector, float maxValue)
    {
      if (vector.magnitude > maxValue)
      {
        vector.Normalize();
        vector *= maxValue;
      }
    }

    private Vector3 Seek(int index, Vector3 target)
    {
      Vector3 dronePosition = flockingDrones[index].rb.position;
      dronePosition.y = 0;
      target.y = 0;

      Vector3 desired = target - dronePosition; // A vector pointing from the position to the target	

      // Scale to maximum speed	
      desired.Normalize();
      desired *= aiMovementControllerStats.MaxVelocity.Value;

      // Steering = Desired minus Velocity	
      Vector3 steer = desired - flockingDrones[index].rb.velocity;

      // Limit to maximum steering force	
      LimitVector(ref steer, aiMovementControllerStats.AccelerationForce.Value);
      return steer;
    }

    private Vector3 Separate(int index)
    {
      Vector3 position = flockingDrones[index].rb.position;
      Vector3 steer = new Vector3(0, 0, 0);

      int inRangeCount = 0;
      for (int i = 0; i < flockingDrones.Count; i++)
      {
        if (i == index) continue;

        // For every boid in the system, check if it's too close	
        float d = Vector3.Distance(position, flockingDrones[i].rb.position);

        // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)	
        if (d < desiredSeparation)
        {
          // Calculate vector pointing away from neighbor	
          Vector3 diff = position - flockingDrones[i].rb.position;
          diff.Normalize();
          diff /= d; // Weight by distance	
          steer += diff;
          inRangeCount++; // Keep track of how many	
        }
      }

      // Average -- divide by how many	
      if (inRangeCount > 0)
      {
        steer /= ((float) inRangeCount);
      }

      // As long as the vector is greater than 0	
      if (steer.sqrMagnitude > 0)
      {
        // Implement Reynolds: Steering = Desired - Velocity	
        steer.Normalize();
        steer *= aiMovementControllerStats.MaxVelocity.Value;
        steer -= flockingDrones[index].rb.velocity;

        if (steer.sqrMagnitude > aiMovementControllerStats.AccelerationForce.Value)
        {
          steer.Normalize();
          steer *= aiMovementControllerStats.AccelerationForce.Value;
        }
      }

      return steer;
    }

    // Alignment	
    // For every nearby boid in the system, calculate the average velocity	
    private Vector3 Align(int index)
    {
      Vector3 sum = new Vector3(0, 0, 0);
      int count = 0;

      for (int i = 0; i < flockingDrones.Count; i++)
      {
        float d = Vector3.Distance(flockingDrones[index].rb.position, flockingDrones[i].rb.position);
        if (d < neighbourDistance)
        {
          sum += (flockingDrones[i].rb.velocity);
          count++;
        }
      }

      if (count > 0)
      {
        sum /= ((float) count);

        // Implement Reynolds: Steering = Desired - Velocity	
        sum.Normalize();
        sum *= aiMovementControllerStats.MaxVelocity.Value;

        Vector3 steer = sum - flockingDrones[index].rb.velocity;
        LimitVector(ref steer, aiMovementControllerStats.AccelerationForce.Value);

        return steer;
      }

      return new Vector3(0, 0, 0);
    }

    // Cohesion	
    // For the average position (i.e. center) of all nearby boids, calculate steering vector towards that position	
    private Vector3 Cohesion(int index)
    {
      Vector3 sum = new Vector3(0, 0, 0); // Start with empty vector to accumulate all positions	
      int count = 0;

      for (int i = 0; i < flockingDrones.Count; i++)
      {
        float d = Vector3.Distance(flockingDrones[index].rb.position, flockingDrones[i].rb.position);
        if (d < neighbourDistance)
        {
          sum += flockingDrones[index].rb.position; // Add position	
          count++;
        }
      }

      if (count > 0)
      {
        sum /= (float) count;
        return Seek(index, sum); // Steer towards the position	
      }

      return new Vector3(0, 0, 0);
    }
  }
}