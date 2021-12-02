using System.Collections.Generic;
using CwispyStudios.TankMania.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace CwispyStudios.TankMania.Enemy
{
  public class DroneFlock : MonoBehaviour
  {
    public List<DroneEnemy> flockingDrones = new List<DroneEnemy>();
    public Transform playerTransform;

    [Header("Component parameters")] [SerializeField]
    private float neighbourDistance;

    [SerializeField] private float desiredSeparation;
    
    //[SerializeField] private float maxSpeed;
    //[SerializeField] private float maxForce;

    [SerializeField] private AIMovementControllerStats aiMovementControllerStats;

    [Header("Component weight")] [SerializeField]
    private float separationFactor = 1.5f;

    [SerializeField] private float alignFactor = 1f;
    [SerializeField] private float cohesionFactor = 1f;
    [SerializeField] private float seekFactor = 4f;
    private List<DroneEnemy> queuedDrones = new List<DroneEnemy>();

    private void AddDrone(DroneEnemy drone)
    {
      queuedDrones.Add(drone);
    }

    private void Update()
    {
      UpdateFlock();
    }

    private void UpdateFlock()
    {
      if (queuedDrones.Count > 0)
      {
        foreach (var drone in queuedDrones)
        {
          flockingDrones.Add(drone);
        }

        queuedDrones.Clear();
      }

      for (int i = 0; i < flockingDrones.Count; i++)
      {
        Vector3 sep = Separate(i); // Separation	
        Vector3 ali = Align(i); // Alignment	
        Vector3 coh = Cohesion(i); // Cohesion	
        Vector3 seek = Seek(i, playerTransform.position);
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
      
      Vector3
        desired = target - dronePosition; // A vector pointing from the position to the target	
      // Scale to maximum speed	
      desired.Normalize();
      desired *= aiMovementControllerStats.MaxVelocity.Value;
      // Above two lines of code below could be condensed with new PVector setMag() method	
      // Not using this method until Processing.js catches up	
      // desired.setMag(maxspeed);	
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
        // First two lines of code below could be condensed with new PVector setMag() method	
        // Not using this method until Processing.js catches up	
        // steer.setMag(maxspeed);	
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
        // First two lines of code below could be condensed with new PVector setMag() method	
        // Not using this method until Processing.js catches up	
        // sum.setMag(maxspeed);	
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