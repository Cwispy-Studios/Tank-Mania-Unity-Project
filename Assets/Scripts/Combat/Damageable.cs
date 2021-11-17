using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  using Stats;

  public class Damageable : MonoBehaviour
  {
    [SerializeField] private TargetType targetType;
    [SerializeField] private Team unitTeam;
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
      return targetType.TargetTeam != team;
    }

    public void TakeDamage( float damage )
    {
      Debug.Log($"{gameObject} took {damage} damage.");
      currentHealth -= damage;

      if (currentHealth < 0f)
      {
        // Remove
      }
    }
  }
}
