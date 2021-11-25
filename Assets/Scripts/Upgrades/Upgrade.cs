using System;
using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  using Stats;

  [CreateAssetMenu(menuName = "Upgrades/Upgrade Instance", order = 0)]
  public class Upgrade : ScriptableObject
  {
    [Header("Upgrade Rarity")]
    public UpgradeRarity UpgradeRarity;

    [Header("Upgrade Descriptors")]
    public string UpgradeName;
    public Sprite UpgradeImage;
    [TextArea(2, 5)] public string UpgradeDescription;

    [Header("Player Upgraders")]
    public List<StatUpgrader> PlayerStatUpgraders = new List<StatUpgrader>();
    public List<StatsCategoryUpgrader> PlayerStatsCategoryUpgraders = new List<StatsCategoryUpgrader>();

    [Header("Enemy Upgraders")]
    public List<StatUpgrader> EnemyStatUpgraders = new List<StatUpgrader>();
    public List<StatsCategoryUpgrader> EnemyStatsCategoryUpgraders = new List<StatsCategoryUpgrader>();

    [NonSerialized] private int playerUpgradedAmount = 0;
    public int PlayerUpgradedAmount => playerUpgradedAmount;

    [NonSerialized] private int enemyUpgradedAmount = 0;
    public int EnemyUpgradedAmount => enemyUpgradedAmount;

    public void UpgradePlayer()
    {
      ++playerUpgradedAmount;

      foreach (StatUpgrader statUpgrader in PlayerStatUpgraders)
        statUpgrader.UpgradeUpgrader();

      foreach (StatsCategoryUpgrader statsCategoryUpgrader in PlayerStatsCategoryUpgraders)
        statsCategoryUpgrader.UpgradeUpgrader();
    }

    public void UpgradeEnemy()
    {
      ++enemyUpgradedAmount;

      foreach (StatUpgrader statModifierInstance in EnemyStatUpgraders)
        statModifierInstance.UpgradeUpgrader();

      foreach (StatsCategoryUpgrader statsCategoryUpgrader in EnemyStatsCategoryUpgraders)
        statsCategoryUpgrader.UpgradeUpgrader();
      }
  }
}
