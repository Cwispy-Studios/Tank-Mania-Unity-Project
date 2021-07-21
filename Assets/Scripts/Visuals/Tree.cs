using System;
using System.ComponentModel;
using UnityEngine;

namespace CwispyStudios.TankMania.Visuals
{
  public class Tree : MonoBehaviour
  {
    private class TreeSegment
    {
      private readonly Transform joint;

      public float force = 0;
      public float velocity = 0;

      private float angle = 0;

      public TreeSegment(Transform joint)
      {
        this.joint = joint;
      }

      public void UpdateSegment(float newForce, float mass, float springConstant,
        float dampingConstant, float timeStep)
      {
        //if (velocity < threshold && angle < threshold)
          //angle = 0;
        
        velocity += (newForce - force) / mass;
        angle += velocity * timeStep;
        force = angle * springConstant + velocity * dampingConstant;

        //print(velocity);

        joint.localRotation = Quaternion.Euler(angle + joint.transform.parent.localEulerAngles.x, 0, 0);
      }
    }

    [SerializeField] private Transform[] joints;
    [SerializeField] private float segmentsMass = 1;
    [SerializeField] private float springConstant = 2;
    [SerializeField] private float dampingConstant = .8f;

    [SerializeField] private float explosionConstant = 1000;
    //[SerializeField] private float centeringForce = 1;

    [SerializeField] private float initialForce = 1;
    
    [SerializeField] private float timeStep = 0.001f;

    private TreeSegment[] segments;

    private void Awake()
    {
      if (joints.Length < 2)
        throw new NullReferenceException("Too few segments, minimum of 2");

      segments = new TreeSegment[joints.Length];

      for (int i = 0; i < joints.Length; i++)
      {
        segments[i] = new TreeSegment(joints[i]);
      }
    }

    public void UpdateTree()
    {
      segments[segments.Length - 1].UpdateSegment(initialForce, segmentsMass, springConstant, dampingConstant, timeStep);

      for (int i = segments.Length - 2; i > 0; i--)
      {
        segments[i].UpdateSegment(initialForce, segmentsMass,
          springConstant, dampingConstant, timeStep);
      }

      segments[0].UpdateSegment(initialForce, segmentsMass, springConstant, dampingConstant, timeStep);

      initialForce = 0;
    }

    public void AddForce(Vector3 position)
    {
      Vector3 direction = position - transform.position;
      transform.rotation = Quaternion.Euler(0, GetAngle(direction), 0);
      initialForce = (1 / Mathf.Pow(direction.magnitude, 2)) * explosionConstant;
    }

    private float GetAngle(Vector3 direction)
    {
      float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

      if (direction.x < 0)
        angle += 360;

      angle += 180;

      return angle;
    }
  }
}