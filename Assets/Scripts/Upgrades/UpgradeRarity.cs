using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Rarity")]
  public class UpgradeRarity : ScriptableObject
  {
    [Range(1, 100)] public int RarityWeight;
    public Color RarityColour;
  }
}
