using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  public class Health : MonoBehaviour
  {
    [SerializeField] private Team unitTeam;
    [SerializeField] private float maxHealth;
    [SerializeField] private float healthRegeneration;
    [SerializeField] private TMPro.TMP_Text healthText;

    private float currentHealth;

    private void Awake()
    {
      currentHealth = maxHealth;
    }

    private void Update()
    {
      if (healthText)
      healthText.text = currentHealth.ToString("F0");
    }

    public bool CanTakeDamageFromTeam( Team team )
    {
      return unitTeam != team;
    }

    public void TakeDamage( float damage )
    {
      Debug.Log($"{gameObject} took {damage} damage.");
      currentHealth -= damage;
    }
  }
}
