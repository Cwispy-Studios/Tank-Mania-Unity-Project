using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class PlayerHealthUI : MonoBehaviour
  {
    [Header("UI Components")]
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private TMP_Text playerHealthText;

    [Header("Player Attributes")]
    [SerializeField] private FloatVariable currentPlayerHealth;
    [SerializeField] private Stat playerMaxHealthStat;

    private void LateUpdate()
    {
      // TODO: Trigger only when health changes
      float currentHealth = currentPlayerHealth.Value;
      float maxHealth = playerMaxHealthStat.Value;

      playerHealthBar.fillAmount = currentHealth / maxHealth;
      playerHealthText.text = $"{currentHealth.ToString("F0")}/{maxHealth.ToString("F0")}";
    }
  }
}
