using System.Collections.Generic;

using UnityEngine;

using TMPro;

namespace CwispyStudios.TankMania.Upgrades
{
  public class UpgradePanel : MonoBehaviour
  {
    [Header("Available Upgrades")]
    [SerializeField] private AvailableUpgrades availableUpgrades;
    [SerializeField] private UpgradedUpgrades upgradedUpgrades;

    [Header("UI Elements")]
    [SerializeField] private UpgradeButton[] upgradeButtons;
    [SerializeField] private TMP_Text upgradeRarityText;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;

    private const int NumberOfPickeableUpgrades = 3;

    private void Awake()
    {
      foreach (UpgradeButton upgradeButton in upgradeButtons)
      {
        upgradeButton.OnHoverEvent += SetDescription;
        upgradeButton.OnNotHoverEvent += ResetDescription;
        upgradeButton.OnSelectEvent += SelectUpgrade;
      }
    }

    public void ShowUpgradesPanel()
    {
      gameObject.SetActive(true);

      // Pause game
      Time.timeScale = 0f;

      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.Confined;

      RandomiseUpgrades();
    }

    private void RandomiseUpgrades()
    {
      List<Upgrade> allUpgradesOfType = new List<Upgrade>(availableUpgrades.Upgrades);

      int sumOfWeights = 0;

      // Find the sum of weights
      foreach (Upgrade upgrade in allUpgradesOfType) sumOfWeights += upgrade.UpgradeRarity.RarityWeight;

      for (int i = 0; i < NumberOfPickeableUpgrades; ++i)
      {
        // Generate a random cumulative weight
        int randomWeighted = Random.Range(0, sumOfWeights);
        int cumulativeWeight = 0;

        Upgrade chosenUpgrade = null;

        foreach (Upgrade upgrade in allUpgradesOfType)
        {
          cumulativeWeight += upgrade.UpgradeRarity.RarityWeight;

          if (randomWeighted < cumulativeWeight)
          {
            chosenUpgrade = upgrade;
            sumOfWeights -= chosenUpgrade.UpgradeRarity.RarityWeight;
            break;
          }
        }

        upgradeButtons[i].SetUpgradeOfButton(chosenUpgrade);
        allUpgradesOfType.Remove(chosenUpgrade);
      }
    }

    public void SetDescription( Upgrade upgrade )
    {
      upgradeRarityText.text = upgrade.UpgradeRarity.name;
      upgradeRarityText.color = upgrade.UpgradeRarity.RarityColour;
      upgradeNameText.text = $"{upgrade.UpgradeName} (+{upgrade.UpgradedAmount})";
      upgradeDescriptionText.text = upgrade.UpgradeDescription;
    }

    public void ResetDescription()
    {
      upgradeRarityText.text = string.Empty;
      upgradeNameText.text = string.Empty;
      upgradeDescriptionText.text = string.Empty;
    }

    public void SelectUpgrade( Upgrade selectedUpgrade )
    {
      selectedUpgrade.UpgradePlayer();

      //upgradedUpgrades.Upgrade(selectedUpgrade);

      // Upgrade enemies
      foreach (UpgradeButton upgradeButton in upgradeButtons)
      {
        Upgrade unselectedUpgrade = upgradeButton.UpgradeComponent;

        if (unselectedUpgrade != selectedUpgrade) unselectedUpgrade.UpgradeEnemy();
      }

      HideUpgradePanel();
    }

    private void HideUpgradePanel()
    {
      gameObject.SetActive(false);

      // Resume game
      Time.timeScale = 1f;

      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

  }
}
