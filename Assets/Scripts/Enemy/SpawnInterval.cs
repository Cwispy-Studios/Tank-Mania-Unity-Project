using UnityEngine;

namespace CwispyStudios.TankMania.Enemy
{
  [CreateAssetMenu(menuName = "Spawning/Spawn Interval")]
  public class SpawnInterval : ScriptableObject
  {
    [Range(1f, 60f)] public float MinIntervalRange = 2f;
    [Range(1f, 60f)] public float MaxIntervalRange = 10f;
  }
}
