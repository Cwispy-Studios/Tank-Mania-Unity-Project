using System;
using System.Collections.Generic;

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
    public List<StatModifierInstance> PlayerStatModifiers = new List<StatModifierInstance>();

    [Header("Enemy Modifiers")]
    public List<StatModifierInstance> EnemyStatModifiers = new List<StatModifierInstance>();

    [NonSerialized] private int playerUpgradedAmount = 0;
    public int PlayerUpgradedAmount => playerUpgradedAmount;

    [NonSerialized] private int enemyUpgradedAmount = 0;
    public int EnemyUpgradedAmount => enemyUpgradedAmount;

    public void UpgradePlayer()
    {
      ++playerUpgradedAmount;

      foreach (StatModifierInstance statModifierInstance in PlayerStatModifiers)
      {
        if (statModifierInstance == null) Debug.LogError("Stat modifier has no subscribers and will not effect any stats!");

        statModifierInstance.UpgradeStatModifierInstance();
      }
    }

    public void UpgradeEnemy()
    {
      ++enemyUpgradedAmount;

      foreach (StatModifierInstance statModifierInstance in EnemyStatModifiers)
      {
        if (statModifierInstance == null) Debug.LogError("Stat modifier has no subscribers and will not effect any stats!");

        statModifierInstance.UpgradeStatModifierInstance();
      }
    }

    public StatModifierInstance GetInstanceFromIndex( int index )
    {
      if (index < PlayerStatModifiers.Count)
        return PlayerStatModifiers[index];

      else if (index < PlayerStatModifiers.Count + EnemyStatModifiers.Count)
        return EnemyStatModifiers[index - PlayerStatModifiers.Count];

      else return null;
    }
  }
}
