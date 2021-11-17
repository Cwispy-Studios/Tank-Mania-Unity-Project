using System;

namespace CwispyStudios.TankMania.Combat
{
  [Serializable]
  public struct TargetType
  {
    public Team TargetTeam;
    public UnitType TargetUnitType;
  }
}
