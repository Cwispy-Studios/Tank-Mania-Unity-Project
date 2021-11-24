namespace CwispyStudios.TankMania.Combat
{
  [System.Flags]
  public enum UnitType
  {
    Ground = 1 << 0,
    Flying = 1 << 1,
    Melee = 1 << 2,
    Ranged = 1 << 3,
  }
}