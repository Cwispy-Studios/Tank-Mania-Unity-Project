using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [System.Serializable]
  public class TargetPreferences
  {
    [Header("Team Targeting")]
    public bool TargetsFriendlies;
    public bool TargetsOpponents;

    [Header("Properties Targeting")]
    public UnitProperties TargetProperties;
  }
}
