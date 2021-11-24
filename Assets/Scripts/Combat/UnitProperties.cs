using System;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public struct UnitProperties
  {
    public Team Team;
    public UnitType UnitType;

    /// <summary>
    /// Checks if this UnitProperties is a soft match with another UnitProperties.
    /// Soft match is defined as belonging to the same team, and a match of ANY UnitType value.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsSoftMatchWith( UnitProperties other )
    {
      // Team must always match
      bool isPreferredTeam = Team == other.Team;
      // If no stated UnitType preference, this will always be true, else check if target meets any selected preference
      bool isPreferredUnitType = UnitType == 0 ? true : (UnitType & other.UnitType) != 0;

      return isPreferredTeam & isPreferredUnitType;
    }

    /// <summary>
    /// Checks if this UnitProperties is a hard match with another UnitProperties.
    /// Hard match is defined as belonging to the same team, and a match of ALL UnitType values with the this UnitProperties.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsHardMatchWith( UnitProperties other )
    {
      // Team must always match
      bool isPreferredTeam = Team == other.Team;
      // If no stated UnitType preference, this will always be true, else check if target meets all the preferences
      bool isPreferredUnitType = UnitType == 0 ? true : (UnitType & other.UnitType) == UnitType;

      return isPreferredTeam & isPreferredUnitType;
    }
  }
}
