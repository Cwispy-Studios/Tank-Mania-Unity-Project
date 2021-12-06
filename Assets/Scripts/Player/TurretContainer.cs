using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/Turret Container")]
  public class TurretContainer : ScriptableObject
  {
    [SerializeField] private List<TurretHub> turrets = new List<TurretHub>();
    public ICollection<TurretHub> ListOfTurrets => turrets.AsReadOnly();
  }
}
