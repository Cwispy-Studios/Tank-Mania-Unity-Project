namespace CwispyStudios.TankMania.Combat
{
  [System.Flags]
  public enum UnitType
  {
    Ground = 1 << 0,
    Flying = 1 << 1
  }
}
