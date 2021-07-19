using System;
using UnityEngine;

namespace Visuals
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

      public void UpdateSegment(float nextForce, float previousVelocity, float mass, float springConstant,
        float dampingConstant, float threshold, float timeStep)
      {
        //if (velocity < threshold && angle < threshold)
          //angle = 0;
        
        velocity += (nextForce - force) / mass;
        velocity -= previousVelocity;
        angle += velocity * timeStep;
        force = angle * springConstant + velocity * dampingConstant;

        //print(velocity);

        joint.rotation = Quaternion.Euler(angle + joint.transform.parent.eulerAngles.x, 0, 0);
      }
    }

    [SerializeField] private Transform[] joints;
    [SerializeField] private float segmentsMass = 1;
    [SerializeField] private float springConstant = 2;
    [SerializeField] private float dampingConstant = .8f;
    //[SerializeField] private float centeringForce = 1;

    [SerializeField] private float initialForce = 1;

    [SerializeField] private float threshold = 1;
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
      segments[segments.Length - 1].UpdateSegment(initialForce, segments[segments.Length - 2].velocity,
        segmentsMass, springConstant, dampingConstant, threshold, timeStep);

      for (int i = segments.Length - 1; i < 1; i--)
      {
        segments[i].UpdateSegment(segments[i+1].force + initialForce, segments[i - 1].velocity, segmentsMass,
          springConstant, dampingConstant, threshold, timeStep);
      }

      segments[0].UpdateSegment(segments[1].force + initialForce, 0, segmentsMass, springConstant, dampingConstant, threshold, timeStep);

      initialForce = 0;
    }

    private float GetExplosionForce()
    {
      return 1f;
    }
  }
}