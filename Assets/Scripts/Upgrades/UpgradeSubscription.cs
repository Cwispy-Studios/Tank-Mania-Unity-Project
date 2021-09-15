using System;

using UnityEngine;

namespace CwispyStudios.TankMania.Upgrades
{
  [Serializable]
  public class UpgradeSubscription
  {
    public StatModifierInstance StatModifierInstance;

    public bool IsValid => StatModifierInstance != null;

#if UNITY_EDITOR
    // QOL to show which upgrade the selected stat modifier instance is from
    [SerializeField] private Upgrade statModifierUpgrade;
    // Selection of upgrade in inspector to choose a stat modifier
    [SerializeField] private Upgrade upgrade;
    // The index of the stat modifier in the upgrade's arrays, -1 for no action to be taken
    [SerializeField] private int statModifierIndex = -1;
    // Whether to show the above properties in the inspector
    [SerializeField] private bool selectingModifier;

    /// <summary>
    /// EDITOR ONLY.
    /// </summary>
    public void AssignStatModifierFromInspectorSelection()
    {
      if (statModifierIndex >= 0)
      {
        // Why does this happen?
        if (upgrade == null)
        {
          statModifierIndex = -1;
          return;
        }

        StatModifierInstance = upgrade.GetInstanceFromIndex(statModifierIndex);
        statModifierUpgrade = upgrade;
        statModifierIndex = -1;
      }
    }

    /// <summary>
    /// EDITOR ONLY.
    /// If an upgrade is deleted, references to its stat modifiers will not get deleted.
    /// This checks for such scenarios and removes references to that stat modifier.
    /// </summary>
    public void ValidateUpgrade()
    {
      if (statModifierUpgrade == null)
      {
        StatModifierInstance = null;
        statModifierIndex = -1;
      }
    }

    private void RemoveStatModifier()
    {
      StatModifierInstance = null;
      statModifierUpgrade = null;
    }

#endif
  }
}
