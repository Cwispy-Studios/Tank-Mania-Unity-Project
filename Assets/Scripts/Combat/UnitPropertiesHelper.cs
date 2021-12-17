namespace CwispyStudios.TankMania.Combat
{
  public static class UnitPropertiesHelper
  {
    /// <summary>
    /// Checks if this UnitProperties is a soft match with another UnitProperties.
    /// Soft match is defined as a match of ANY UnitProperties value.
    /// </summary>
    /// <param name="other">
    /// UnitProperties of the other object to check against.
    /// </param>
    /// <returns>
    /// If it is a soft match.
    /// </returns>
    public static bool IsSoftMatchWith( this UnitProperties unitProperties, UnitProperties other )
    {
      // If no stated UnitType preference, this will always be true, else check if target meets any selected preference
      bool isPreferredUnitType = unitProperties == 0 ? true : (unitProperties & other) != 0;

      return isPreferredUnitType;
    }

    /// <summary>
    /// Checks if this UnitProperties is a hard match with another UnitProperties.
    /// Hard match is defined as a match of ALL UnitProperties values with the this UnitProperties.
    /// </summary>
    /// <param name="other">
    /// UnitProperties of the other object to check against.
    /// </param>
    /// <returns>
    /// If it is a hard match.
    /// </returns>
    public static bool IsHardMatchWith( this UnitProperties unitProperties, UnitProperties other )
    {
      // If no stated UnitType preference, this will always be true, else check if target meets all the preferences
      bool isPreferredUnitType = unitProperties == 0 ? true : (unitProperties & other) == unitProperties;

      return isPreferredUnitType;
    }
  }
}
