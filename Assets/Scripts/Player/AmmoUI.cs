using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.TankMania.Player
{
  using Stats;

  public class AmmoUI : MonoBehaviour
  {
    [Tooltip("Whether to use stacked ammo images or single-file ammo images")]
    [SerializeField] private bool useStackedAmmoImages;
    [SerializeField] private AmmoImageGroup ammoImageGroupPrefab;
    [SerializeField] private AmmoImage ammoImagePrefab;

    [Header("Turret Stats and Attributes"), Tooltip("Firing information of the desired turret to track")]
    [SerializeField] private FiringInformation firingInformation;
    [SerializeField] private FloatVariable reloadCountdown;
    [SerializeField] private IntVariable currentAmmo;
    
    // List of ammo groups which each contain a reticleImage
    public List<AmmoImageGroup> ammoImageGroups = new List<AmmoImageGroup>();
    // List of individual reticle image
    // When using ammoGroups, this provides an easy way to retrieve ammoImages without further calculations
    // This will then also hold inactive ammoImages
    public List<AmmoImage> ammoImages = new List<AmmoImage>();

    private int previousCurrentAmmo;

    private void Awake()
    {
      InitialiseUI();

      firingInformation.AmmoCount.OnStatUpgrade += OnAmmoCountChange;
    }

    private void InitialiseUI()
    {
      if (useStackedAmmoImages)
      {
        int numberOfAmmoGroup = Mathf.CeilToInt(
          (float)firingInformation.AmmoCount.Value / (float)ammoImageGroupPrefab.AmmoImagesPerColumn);

        AddAmmoGroups(numberOfAmmoGroup);
      }

      else
      {
        for (int i = 0; i < firingInformation.AmmoCount.Value; ++i)
        {
          AmmoImage ammoImage = Instantiate(ammoImagePrefab, transform);

          ammoImages.Add(ammoImage);
        }
      }

      previousCurrentAmmo = firingInformation.AmmoCount.Value - 1;
    }

    private void AddAmmoGroups( int amount )
    {
      for (int i = 0; i < amount; ++i)
      {
        AmmoImageGroup reticleGroup = Instantiate(ammoImageGroupPrefab, transform);
        reticleGroup.Initialise();
        reticleGroup.SetAllActive(true);

        ammoImageGroups.Add(reticleGroup);

        for (int j = 0; j < ammoImageGroups[i].AmmoImagesPerColumn; ++j) ammoImages.Add(reticleGroup[j]);
      }

      SetLastAmmoGroupImagesActive();
    }

    private void SetLastAmmoGroupImagesActive()
    {
      AmmoImageGroup lastAmmoImageGroup = ammoImageGroups[ammoImageGroups.Count - 1];

      if (firingInformation.AmmoCount.Value % ammoImageGroupPrefab.AmmoImagesPerColumn == 0)
        lastAmmoImageGroup.SetAllActive(true);

      else
      {
        int totalInAmmoGroups = ammoImageGroups.Count * ammoImageGroupPrefab.AmmoImagesPerColumn;
        int remainder = totalInAmmoGroups - firingInformation.AmmoCount.Value;

        lastAmmoImageGroup.SetAmountActive(remainder);
      }
    }

    private void Update()
    {
      UpdateAmmo();
    }
   
    private void UpdateAmmo()
    {
      // Prevent overflow at max ammo
      if (currentAmmo.Value == firingInformation.AmmoCount.Value) return;

      AmmoImage currentAmmoImage = ammoImages[currentAmmo.Value];

      if (previousCurrentAmmo != currentAmmo.Value)
      {
        // Ammo reloaded, completely fill old ammo
        if (previousCurrentAmmo < currentAmmo.Value)
          ammoImages[previousCurrentAmmo].SetFill(1f);

        // Ammo used, completely empty old ammo
        else
          ammoImages[previousCurrentAmmo].SetFill(0f);

        previousCurrentAmmo = currentAmmo.Value;
      }

      currentAmmoImage.SetFill(1f - (reloadCountdown.Value / firingInformation.ReloadSpeed.Value));
    }

    private void OnAmmoCountChange()
    {
      int newAmmoCount = firingInformation.AmmoCount.Value;

      if (useStackedAmmoImages)
      {
        int numberOfNewAmmoGroups = Mathf.CeilToInt(
          (float)firingInformation.AmmoCount.Value / (float)ammoImageGroupPrefab.AmmoImagesPerColumn);

        // Some ammo groups should be removed
        if (numberOfNewAmmoGroups < ammoImageGroups.Count)
        {
          for (int i = ammoImageGroups.Count - 1; i >= numberOfNewAmmoGroups; --i)
          {
            Destroy(ammoImageGroups[i].gameObject);
            ammoImageGroups.RemoveAt(i);

            ammoImages.RemoveRange(ammoImages.Count - ammoImageGroupPrefab.AmmoImagesPerColumn, ammoImageGroupPrefab.AmmoImagesPerColumn);
          }

          SetLastAmmoGroupImagesActive();
        }

        // Some ammo groups should be added
        else if (numberOfNewAmmoGroups > ammoImageGroups.Count)
        {
          // Find how many to add
          int numberToAdd = numberOfNewAmmoGroups - ammoImageGroups.Count;

          // Set the last ammo group's images to all be active
          AmmoImageGroup lastAmmoImageGroup = ammoImageGroups[ammoImageGroups.Count - 1];
          lastAmmoImageGroup.SetAllActive(true);

          AddAmmoGroups(numberToAdd);
        }

        else SetLastAmmoGroupImagesActive();
      }

      else
      {
        // Some ammoImages should be removed
        if (newAmmoCount < ammoImages.Count)
        {
          for (int i = ammoImageGroups.Count - 1; i > newAmmoCount; --i)
          {
            Destroy(ammoImages[i]);
            ammoImages.RemoveAt(i);
          }
        }

        // Some ammoImages should be added
        else
        {
          int numberToAdd = newAmmoCount - ammoImages.Count;

          for (int i = 0; i < numberToAdd; ++i)
          {
            AmmoImage ammoImage = Instantiate(ammoImagePrefab, transform);

            ammoImages.Add(ammoImage);
          }
        }
      }

      // If ammo is removed, this will go out of bounds
      previousCurrentAmmo = Mathf.Clamp(previousCurrentAmmo, 0, firingInformation.AmmoCount.Value - 1);
    }
  }
}
