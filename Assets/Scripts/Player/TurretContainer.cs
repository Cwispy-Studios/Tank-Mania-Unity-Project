using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/Turret Container")]
  public class TurretContainer : ScriptableObject
  {
    [SerializeField] private List<TurretController> turrets = new List<TurretController>();
    public ICollection<TurretController> ListOfTurrets => turrets.AsReadOnly();
  }
}
