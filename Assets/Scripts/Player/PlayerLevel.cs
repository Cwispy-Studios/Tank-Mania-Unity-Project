using UnityEngine;
using System;

namespace CwispyStudios.TankMania.Player
{
  public class PlayerLevel : MonoBehaviour
  {
    private IntVariable playerLevel;
    private FloatVariable playerExperience;

    private static float ExperiencePerLevel = 500f;

    private float currentExperience = 0f;
  }
}
