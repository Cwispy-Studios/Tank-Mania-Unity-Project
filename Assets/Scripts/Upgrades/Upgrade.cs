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

    [Header("Player Modifiers"), SerializeField]
    public List<StatModifierInstance> PlayerStatModifiers = new List<StatModifierInstance>();

    [Header("Enemy Modifiers"), SerializeField]
    public List<StatModifierInstance> EnemyStatModifiers = new List<StatModifierInstance>();

    [NonSerialized] private int playerUpgradedAmount = 0;
    public int PlayerUpgradedAmount => playerUpgradedAmount;

    [NonSerialized] private int enemyUpgradedAmount = 0;
    public int EnemyUpgradedAmount => enemyUpgradedAmount;

#if UNITY_EDITOR
    private List<StatModifierInstance> oldStatModifiers = new List<StatModifierInstance>();

    private void OnValidate()
    {
      UpdateOldStatModifiersList();
    }

    private void UpdateOldStatModifiersList()
    {
      // Loop through the old list...
      for (int i = oldStatModifiers.Count - 1; i >= 0; --i)
      {
        StatModifierInstance statModifier = oldStatModifiers[i];

        // ...and check if any stat modifier instance has been deleted and should no longer exist
        if (!PlayerStatModifiers.Contains(statModifier) && !EnemyStatModifiers.Contains(statModifier))
        {
          statModifier.OnInstanceRemoved?.Invoke();
          oldStatModifiers.RemoveAt(i);
        }
      }

      // Update the old list 
      oldStatModifiers.Clear();
      oldStatModifiers.AddRange(PlayerStatModifiers);
      oldStatModifiers.AddRange(EnemyStatModifiers);
    }

#endif

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
