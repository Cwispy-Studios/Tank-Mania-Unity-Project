using UnityEngine;

namespace CwispyStudios.TankMania.Combat
{
  /// <summary>
  /// Handles layer mask and team checking
  /// </summary>
  public class UnitTeam : MonoBehaviour
  {
    public static int PlayerLayer = 10;
    public static int PlayerProjectileLayer = 11;
    public static int EnemyLayer = 12;
    public static int EnemyProjectileLayer = 13;

    private static LayerMask PlayerLayerMask = 1 << PlayerLayer;
    private static LayerMask EnemyLayerMask = 1 << EnemyLayer;

    private LayerMask thisLayerMask;
    private LayerMask opponentLayerMask;

    public LayerMask ThisLayerMask => thisLayerMask;
    public LayerMask OpponentLayerMask => opponentLayerMask;

    private void Awake()
    {
      int unitLayer = gameObject.layer;

      // Unit belongs to Player
      if (unitLayer == PlayerLayer || unitLayer == PlayerProjectileLayer)
      {
        thisLayerMask = PlayerLayerMask;
        opponentLayerMask = EnemyLayerMask;
      }

      // Unit belongs to Enemy
      else if (unitLayer == EnemyLayer || unitLayer == EnemyProjectileLayer)
      {
        thisLayerMask = EnemyLayerMask;
        opponentLayerMask = PlayerLayerMask;
      }

      // Unit belongs to neither, may have forgotten to assign proper layer
      else
      {
        Debug.LogWarning($"Unit's layer {unitLayer} does not belong to any team layer!", this);
      }
    }

    /// <summary>
    /// Checks if another object belongs on the same team. DOES NOT CHECK PROJECTILES
    /// </summary>
    /// <param name="other">
    /// The other object to check against.
    /// </param>
    /// <returns>
    /// Whether the other object belongs on the same team.
    /// </returns>
    public bool OnSameTeam( GameObject other )
    {
      int otherLayer = other.layer;

      LayerMask otherLayerMask = 1 << otherLayer;

      return ((thisLayerMask & otherLayerMask) != 0);
    }

    /// <summary>
    /// Checks if another object belongs on the opposing team. DOES NOT CHECK PROJECTILES
    /// </summary>
    /// <param name="other">
    /// The other object to check against.
    /// </param>
    /// <returns>
    /// Whether the other object belongs on the opposing team.
    /// </returns>
    public bool OnOpposingTeam( GameObject other )
    {
      int otherLayer = other.layer;

      LayerMask otherLayerMask = 1 << otherLayer;

      return ((opponentLayerMask & otherLayerMask) != 0);
    }
  }
}
