using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.Player
{
  public class GunTurretsMenuUI : MonoBehaviour
  {
    [SerializeField] private TurretsSlotsHandler turretsSlotsHandler;

    [SerializeField] private Transform turretsMenu;

    [Header("List Contents")]
    [SerializeField] private Transform gunSlotsContent;
    [SerializeField] private Transform turretsContent;

    [Header("Button Prefabs")]
    [SerializeField] private SlotButtonUI slotButtonPrefab;
    [SerializeField] private TurretButtonUI turretButtonPrefab;

    private TurretSlot selectedSlot = null;

    private void OnEnable()
    {
      CreateUI();
    }

    private void OnDisable()
    {
      DestroyUI();

      selectedSlot = null;
      turretsMenu.gameObject.SetActive(false);
    }

    private void CreateUI()
    {
      // Create gun slots list
      foreach (TurretSlot turretSlot in turretsSlotsHandler.ListOfTurretSlots)
      {
        SlotButtonUI button = Instantiate(slotButtonPrefab, gunSlotsContent);
        button.SetContent(turretSlot, turretsSlotsHandler.GetTurretOfSlot(turretSlot));
        button.GetComponent<Button>().interactable = !(turretSlot == selectedSlot);
        button.OnClick += SetSelectedSlot;
      }

      // Create turrets list
      foreach (TurretHub turret in turretsSlotsHandler.UnlockedTurrets)
      {
        bool turretIsAssigned = turretsSlotsHandler.IsTurretAssigned(turret);

        TurretButtonUI button = Instantiate(turretButtonPrefab, turretsContent);
        button.SetContent(turret, turretsSlotsHandler.GetSlotOfTurret(turret));
        button.GetComponent<Button>().interactable = !turretIsAssigned;

        button.OnClickEvent += AssignTurretToSlot;
        button.OnUnassignEvent += UnassignTurret;
      }
    }

    private void DestroyUI()
    {
      foreach (Transform t in gunSlotsContent)
      {
        Destroy(t.gameObject);
      }

      foreach (Transform t in turretsContent)
      {
        Destroy(t.gameObject);
      }
    }

    public void RefreshUI()
    {
      DestroyUI();
      CreateUI();
    }

    private void AssignTurretToSlot( TurretHub turret )
    {
      turretsSlotsHandler.AssignTurretToSlot(turret, selectedSlot);

      RefreshUI();
    }

    private void UnassignTurret( TurretHub turret )
    {
      turretsSlotsHandler.UnassignTurret(turret);

      RefreshUI();
    }

    /////////////////////
    /// Button callbacks

    private void SetSelectedSlot( TurretSlot slot )
    {
      turretsMenu.gameObject.SetActive(true);
      selectedSlot = slot;

      RefreshUI();
    }
  }
}
