using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public class Damageable : MonoBehaviour
  {
    [SerializeField] private UnitProperties unitProperties;
    public UnitProperties UnitProperties => unitProperties;

    [SerializeField] private Health health;

    [Header("Debug")]
    // DEBUG
    [SerializeField] private TMPro.TMP_Text healthText;

    protected float currentHealth;

    /// <summary>
    /// Used by TargetFinder to remove object when it is disabled
    /// </summary>
    public event Action<Damageable> OnObjectDie;

    private void OnEnable()
    {
      currentHealth = health.MaxHealth.Value;
    }

    private void Update()
    {
      if (health.HealthRegeneration.Value > 0f && currentHealth < health.MaxHealth.Value)
        RegenerateHealth();

      // DEBUG
      if (healthText)
        healthText.text = currentHealth.ToString("F0");
    }

    private void RegenerateHealth()
    {
      currentHealth += health.HealthRegeneration.Value * Time.deltaTime;
      currentHealth = Mathf.Clamp(currentHealth, 0f, health.MaxHealth.Value);
    }

    public bool CanTakeDamageFromTeam( Team team )
    {
      return unitProperties.Team != team;
    }

    public void TakeDamage( float damage )
    {
      Debug.Log($"{gameObject} took {damage} damage.");
      currentHealth -= damage;

      if (currentHealth < 0f)
      {
        Die();
      }
    }

    protected virtual void Die()
    {
      OnObjectDie?.Invoke(this);

      gameObject.SetActive(false);
    }
  }
}
