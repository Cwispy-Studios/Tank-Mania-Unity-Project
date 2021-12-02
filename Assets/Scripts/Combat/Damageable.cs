using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public class Damageable : PooledObject
  {
    [SerializeField] private UnitProperties unitProperties;
    public UnitProperties UnitProperties => unitProperties;
    [SerializeField] private Health health;

    // DEBUG
    [SerializeField] private TMPro.TMP_Text healthText;

    private float currentHealth;

    private void Awake()
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
        gameObject.SetActive(false);
        // Remove
      }
    }
  }
}
