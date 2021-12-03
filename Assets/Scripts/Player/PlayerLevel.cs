using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using GameEvents;

  public class PlayerLevel : MonoBehaviour
  {
    [Header("Level and Experience")]
    [SerializeField] private IntVariable playerLevel;
    [SerializeField] private IntVariable playerExperience;
    [SerializeField] private IntVariable experienceToNextLevel;

    [Header("Events")]
    [SerializeField] private VoidEvent onPlayerLevelUp;

    private static int ExperiencePerLevel = 50;

    private void Awake()
    {
      playerLevel.Value = 1;
      playerExperience.Value = 0;

      CalculateExperienceNeededToNextLevel();
    }

    private void CalculateExperienceNeededToNextLevel()
    {
      experienceToNextLevel.Value = playerLevel.Value * ExperiencePerLevel;
    }

    public void GainExperience( int experience )
    {
      playerExperience.Value += experience;

      if (playerExperience.Value >= experienceToNextLevel.Value) LevelUp();
    }

    private void LevelUp()
    {
      ++playerLevel.Value;
      playerExperience.Value -= experienceToNextLevel.Value;

      CalculateExperienceNeededToNextLevel();

      onPlayerLevelUp.Raise();
    }
  }
}
