using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
  public class BirdoFlock : MonoBehaviour
  {
    public List<BirdoEnemy> flockingBirdos = new List<BirdoEnemy>();
    public Transform playerTransform;

    [SerializeField] private float neighbourDistance;
    [SerializeField] private float desiredSeparation;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxForce;

    [SerializeField] private float separationFactor = 1.5f;
    [SerializeField] private float alignFactor = 1f;
    [SerializeField] private float cohesionFactor = 1f;
    [SerializeField] private float seekFactor = 4f;


    private void Start()
    {
      InvokeRepeating(nameof(UpdateFlock), 1, .2f);
    }

    private void UpdateFlock()
    {
      for (int i = 0; i < flockingBirdos.Count; i++)
      {
        Vector3 sep = Separate(i);   // Separation
        Vector3 ali = Align(i);      // Alignment
        Vector3 coh = Cohesion(i);   // Cohesion
        Vector3 seek = Seek(i, playerTransform.position);
        // Arbitrarily weight these forces
        sep *= separationFactor;
        ali *= alignFactor;
        coh *= cohesionFactor;
        seek *= seekFactor;
        // Add the force vectors to acceleration
        flockingBirdos[i].ApplyForce(sep, maxSpeed);
        flockingBirdos[i].ApplyForce(ali, maxSpeed);
        flockingBirdos[i].ApplyForce(coh, maxSpeed);
        flockingBirdos[i].ApplyForce(seek, maxSpeed);
      }
    }

    private Vector3 LimitVector(ref Vector3 vector, float maxValue)
    {
      if (vector.magnitude > maxValue)
      {
        vector.Normalize();
        vector *= maxValue;
      }

      return vector;
    }

    private Vector3 Seek(int index, Vector3 target)
    {
      Vector3 desired = target - flockingBirdos[index].rb.position; // A vector pointing from the position to the target
      // Scale to maximum speed
      desired.Normalize();
      desired *= maxSpeed;

      // Above two lines of code below could be condensed with new PVector setMag() method
      // Not using this method until Processing.js catches up
      // desired.setMag(maxspeed);

      // Steering = Desired minus Velocity
      Vector3 steer = desired - flockingBirdos[index].rb.velocity;

      // Limit to maximum steering force
      LimitVector(ref steer, maxForce);

      return steer;
    }

    private Vector3 Separate(int index)
    {
      Vector3 position = flockingBirdos[index].rb.position;
      Vector3 steer = new Vector3(0, 0, 0);

      int inRangeCount = 0;

      for (int i = 0; i < flockingBirdos.Count; i++)
      {
        if (i == index) continue;

        // For every boid in the system, check if it's too close
        float d = Vector3.Distance(position, flockingBirdos[i].rb.position);
        // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
        if (d < desiredSeparation)
        {
          // Calculate vector pointing away from neighbor
          Vector3 diff = position - flockingBirdos[i].rb.position;
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
      if (steer.magnitude > 0)
      {
        // First two lines of code below could be condensed with new PVector setMag() method
        // Not using this method until Processing.js catches up
        // steer.setMag(maxspeed);

        // Implement Reynolds: Steering = Desired - Velocity
        steer.Normalize();
        steer *= maxSpeed;
        steer -= flockingBirdos[index].rb.velocity;

        if (steer.magnitude > maxForce)
        {
          steer.Normalize();
          steer *= maxForce;
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

      for (int i = 0; i < flockingBirdos.Count; i++)
      {
        float d = Vector3.Distance(flockingBirdos[index].rb.position, flockingBirdos[i].rb.position);
        if (d < neighbourDistance)
        {
          sum += (flockingBirdos[i].rb.velocity);
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
        sum *= maxSpeed;
        Vector3 steer = sum - flockingBirdos[index].rb.velocity;


        LimitVector(ref steer, maxForce);
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

      for (int i = 0; i < flockingBirdos.Count; i++)
      {
        float d = Vector3.Distance(flockingBirdos[index].rb.position, flockingBirdos[i].rb.position);
        if (d < neighbourDistance)
        {
          sum += flockingBirdos[index].rb.position; // Add position
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