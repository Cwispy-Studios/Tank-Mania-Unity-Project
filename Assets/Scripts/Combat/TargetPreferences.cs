using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  [System.Serializable]
  public class TargetPreferences
  {
    [Header("Team Targeting")]
    public bool TargetsFriendlies;
    public bool TargetsOpponents;

    [Header("Properties Targeting"), Tooltip("Target can contain ANY of the properties selected. If it is None (0), accepts anything.")]
    public UnitProperties TargetProperties;
  }
}
