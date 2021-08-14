using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [CreateAssetMenu(menuName = "Upgrades/Upgrade Instance", order = 0)]
  public class Upgrade : ScriptableObject
  {
    [Header("Upgrade Descriptors")]
    public string UpgradeName;
    public Sprite UpgradeImage;
    [TextArea(2, 5)] public string UpgradeDescription;

    [Header("Player Modifiers")]
    public StatModifier[] PlayerStatModifiers;

    [Header("Enemy Modifiers")]
    public StatModifier[] EnemyStatModifiers;

    private int upgradedAmount = 0;

    public void UpgradePlayer()
    {
      ++upgradedAmount;

      foreach (StatModifier statModifier in PlayerStatModifiers)
      {
        statModifier.Upgrade();
      }
    }

    public void UpgradeEnemy()
    {

    }
  }
}
