using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  public class Health : MonoBehaviour
  {
    [SerializeField] private Team unitTeam;
    [SerializeField, Range(0f, 10000f)] private float maxHealth;
    [SerializeField, Range(0f, 100f)] private float healthRegeneration;

    // DEBUG
    [SerializeField] private TMPro.TMP_Text healthText;

    private float currentHealth;

    private void Awake()
    {
      currentHealth = maxHealth;
    }

    private void Update()
    {
      if (healthRegeneration > 0f && currentHealth < maxHealth) RegenerateHealth();

      // DEBUG
      if (healthText)
        healthText.text = currentHealth.ToString("F0");
    }

    private void RegenerateHealth()
    {
      currentHealth += healthRegeneration * Time.deltaTime;
      currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public bool CanTakeDamageFromTeam( Team team )
    {
      return unitTeam != team;
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
