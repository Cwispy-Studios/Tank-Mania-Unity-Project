using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.TankMania.Player
{
  public class ExperienceBarUI : MonoBehaviour
  {
    [Header("UI Components")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private Image experienceBar;

    [Header("Player Attributes")]
    [SerializeField] private IntVariable playerLevel;
    [SerializeField] private IntVariable playerExperience;
    [SerializeField] private IntVariable experienceToNextLevel;

    private void LateUpdate()
    {
      // TODO: Trigger only when health changes
      levelText.text = $"Level {playerLevel.Value}";
      experienceText.text = $"{playerExperience.Value}/{experienceToNextLevel.Value}";

      experienceBar.fillAmount = playerExperience.Value / experienceToNextLevel.Value;
    }
  }
}
