using UnityEngine;

namespace CwispyStudios.TankMania.Stats
{
  [CreateAssetMenu(menuName = "Stats/Player/AI Turret Stats")]
  public class AITurretStats : StatsGroup
  {
    [StatRange(2f, 100f), Tooltip("Range the AI turret can detect enemies.")]
    public Stat DetectionRange;

    [StatRange(0f, 20f), Tooltip("Range where the AI turret cannot shoot at enemies.")]
    public Stat MinDetectionRange;

    [StatRange(0f, 20f), Tooltip("Angular distance where the turret will still fire at its target before it has faced it.")]
    public Stat Imprecision;
  }
}
