using System;
using CwispyStudios.TankMania.Combat;
using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  [RequireComponent(typeof(Damageable))]
  public class Enemy : MonoBehaviour
  {
    protected Damageable damageable;

    private void Awake()
    {
      damageable = GetComponent<Damageable>();
    }
  }
}