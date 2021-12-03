using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  using GameEvents;

  public class Enemy : MonoBehaviour
  {
    [Header("Events")]
    [SerializeField] private IntEvent onEnemyKilled;

    [Header("Attributes")]
    [SerializeField, Min(0f)] private int experiencePoints;

    private void OnDisable()
    {
      onEnemyKilled.Raise(experiencePoints);
    }
  }
}
