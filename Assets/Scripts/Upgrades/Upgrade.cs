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
    public List<StatUpgrader> PlayerStatUpgraders = new List<StatUpgrader>();

    [Header("Enemy Modifiers")]
    public List<StatUpgrader> EnemyStatUpgraders = new List<StatUpgrader>();

    [NonSerialized] private int playerUpgradedAmount = 0;
    public int PlayerUpgradedAmount => playerUpgradedAmount;

    [NonSerialized] private int enemyUpgradedAmount = 0;
    public int EnemyUpgradedAmount => enemyUpgradedAmount;

    public void UpgradePlayer()
    {
      ++playerUpgradedAmount;

      foreach (StatUpgrader statUpgrader in PlayerStatUpgraders)
      {
        if (statUpgrader.StatUpgraded == null || statUpgrader.StatModifier == null) 
          Debug.LogError("Stat upgrader has is either not upgrading any stats or has no modifier!");

        statUpgrader.UpgradeStatModifierInstance();
      }
    }

    public void UpgradeEnemy()
    {
      ++enemyUpgradedAmount;

      foreach (StatUpgrader statModifierInstance in EnemyStatUpgraders)
      {
        if (statModifierInstance == null) Debug.LogError("Stat modifier has no subscribers and will not effect any stats!");

        statModifierInstance.UpgradeStatModifierInstance();
      }
    }

    public StatUpgrader GetInstanceFromIndex( int index )
    {
      if (index < PlayerStatUpgraders.Count)
        return PlayerStatUpgraders[index];

      else if (index < PlayerStatUpgraders.Count + EnemyStatUpgraders.Count)
        return EnemyStatUpgraders[index - PlayerStatUpgraders.Count];

      else return null;
    }
  }
}
