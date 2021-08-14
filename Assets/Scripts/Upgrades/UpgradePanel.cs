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

      for (int i = 0; i < NumberOfPickeableUpgrades; ++i)
      {
        int randomIndex = Random.Range(0, allUpgradesOfType.Count);

        upgradeButtons[i].SetUpgradeOfButton(allUpgradesOfType[randomIndex]);
        allUpgradesOfType.RemoveAt(randomIndex);
      }
    }

    public void SetDescription( Upgrade upgrade )
    {
      upgradeNameText.text = upgrade.UpgradeName;
      upgradeDescriptionText.text = upgrade.UpgradeDescription;
    }

    public void ResetDescription()
    {
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
