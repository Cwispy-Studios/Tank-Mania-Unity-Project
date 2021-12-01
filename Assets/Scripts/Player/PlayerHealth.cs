using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Combat;

  public class PlayerHealth : Damageable
  {
    [Header("Accessible Layer")]
    [SerializeField] private FloatVariable currentPlayerHealth;

    private void Start()
    {
      currentPlayerHealth.Value = currentHealth;
    }

    private void LateUpdate()
    {
      currentPlayerHealth.Value = currentHealth;
    }
  }
}
