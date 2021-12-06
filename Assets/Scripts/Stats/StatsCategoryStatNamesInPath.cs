#if UNITY_EDITOR

namespace CwispyStudios.TankMania.Stats
{
  /// <summary>
  /// This class is a utility class for an inspector extention of StatsCategory 
  /// and should not be used in any run-time function.
  /// </summary>
  [System.Serializable]
  public struct StatsCategoryStatNamesInPath
  {
    public string StatName;
    public string FindInsidePath;
  }
}

#endif