using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/Player Movement")]
  public class PlayerMovement : ScriptableObject
  {
    [Header("Acceleration")]
    [Range(2f, 5f)] public float MaxVelocity = 4.5f;
    [Range(5f, 25f)] public float AccelerationForce = 15f;

    [Header("Steering")]
    [Range(1f, 5f)] public float MaxTorque = 1.2f;
    [Range(0.5f, 5f)] public float SteerForce = 1f;
    [Range(1f, 5f)] public float SteerNullifierForceModifier = 3.5f;
  }
}
