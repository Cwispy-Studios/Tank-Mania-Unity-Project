using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  [CreateAssetMenu(menuName = "Player/TurretContainer")]
  public class TurretContainer : ScriptableObject
  {
    [SerializeField] private List<TurretHub> turrets;
    public ICollection<TurretHub> ListOfTurrets => turrets.AsReadOnly();
  }
}
