using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using Combat;
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

    private Damageable damageable;

    private void Awake()
    {
      damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
      damageable.OnObjectDie += GiveExperience;
    }

    private void OnDisable()
    {
      damageable.OnObjectDie -= GiveExperience;
    }

    private void GiveExperience( Damageable _ )
    {
      onEnemyKilled.Raise(experiencePoints);
    }
  }
}
