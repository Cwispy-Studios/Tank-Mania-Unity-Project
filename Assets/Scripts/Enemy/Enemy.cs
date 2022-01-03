using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using GameEvents;

  public class Enemy : MonoBehaviour
  {
    [Header("Events")]
    [SerializeField] private IntEvent onEnemyKilled;

    [Header("Attributes")]
    [Tooltip("How much experience points is awarded to the player when this enemy is killed.")]
    [SerializeField, Min(0f)] private int experiencePoints;
    [Tooltip("The cost of spawning this enemy for the director.")]
    [SerializeField, Min(0f)] private int creditsCost;

    private void OnDisable()
    {
      onEnemyKilled.Raise(experiencePoints);
    }
  }
}
