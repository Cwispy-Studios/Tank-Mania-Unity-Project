using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Available Upgrades")]
  public class AvailableUpgrades : ScriptableObject
  {
    public Upgrade[] Upgrades;
  }
}
