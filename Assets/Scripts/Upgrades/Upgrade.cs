using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Upgrade Instance", order = 0)]
  public class Upgrade : ScriptableObject
  {
    [Header("Upgrade Rarity")]
    public UpgradeRarity UpgradeRarity;

    [Header("Upgrade Descriptors")]
    public string UpgradeName;
    public Sprite UpgradeImage;
    [TextArea(2, 5)] public string UpgradeDescription;

    [Header("Player Modifiers")]
    public StatModifier[] PlayerStatModifiers;

    [Header("Enemy Modifiers")]
    public StatModifier[] EnemyStatModifiers;

    [System.NonSerialized] private int upgradedAmount = 0;
    public int UpgradedAmount => upgradedAmount;

    public void UpgradePlayer()
    {
      ++upgradedAmount;

      foreach (StatModifier statModifier in PlayerStatModifiers)
      {
        if (statModifier == null) Debug.LogError("Stat modifier has no subscribers and will not effect any stats!", statModifier);

        statModifier.Upgrade();
      }
    }

    public void UpgradeEnemy()
    {
      foreach (StatModifier statModifier in EnemyStatModifiers)
      {
        if (statModifier == null) Debug.LogError("Stat modifier has no subscribers and will not effect any stats!", statModifier);

        statModifier.Upgrade();
      }
    }
  }
}
